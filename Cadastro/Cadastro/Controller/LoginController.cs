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
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var resultado = await _loginUseCase.ExecutarAsync(loginDto);

        if (!resultado.IsSuccess)
        {
            // Unauthorized (401) é o código correto para erro de login
            return Unauthorized(new { Message = resultado.ErrorMessage });
        }

        return Ok(new
        {
            Mensagem = "Login realizado com sucesso!",
            Usuario = resultado.Value
        });
    }
}