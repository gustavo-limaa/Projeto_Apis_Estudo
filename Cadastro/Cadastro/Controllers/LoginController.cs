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
        var resultado = await _loginUseCase.ExecutarAsync(loginDto);

        if (!resultado.IsSuccess)
        {
            return Unauthorized(new { Message = resultado.ErrorMessage });
        }

        // Como resultado.Value é um LoginResponseDto,
        // o ASP.NET faz o "match" automático com o ActionResult<LoginResponseDto>
        return Ok(resultado.Value);
    }
}