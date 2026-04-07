using Cadastro.Dtos.LoginDtos;
using Cadastro.UseCases.LoginCases;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly LoginUsuarioUseCase _loginUseCase;

    public LoginController(LoginUsuarioUseCase loginUseCase)
    {
        _loginUseCase = loginUseCase;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        if (loginDto == null) { return BadRequest(new { Message = " Todos os Campos Tem q ser preenchido por favor preencha os campos " }); }

        var resultado = await _loginUseCase.ExecutarAsync(loginDto);

        if (!resultado.IsSuccess)
        {
            return Unauthorized(new { Message = resultado.ErrorMessage });
        }

        // Como resultado.Value é um LoginResponseDto,
        // o ASP.NET faz o "match" automático com o ActionResult<LoginResponseDto>
        return Ok(resultado.Value);
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<LoginResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenDto, [FromServices] RefreshTokenUsesCases refreshTokenUseCases)
    {
        if (refreshTokenDto == null || string.IsNullOrEmpty(refreshTokenDto.RefreshToken))
        {
            return BadRequest(new { Message = "Refresh token é obrigatório." });
        }

        var resultado = await refreshTokenUseCases.ExecutarAsync(refreshTokenDto);
        if (!resultado.IsSuccess)
        {
            return Unauthorized(new { Message = resultado.ErrorMessage });
        }
        return Ok(resultado.Value);
    }
}