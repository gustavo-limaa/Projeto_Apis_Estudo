using Cadastro.Dados;
using Cadastro.Dtos.LoginDtos;
using Cadastro.Dtos.UsuarioDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            // 1. Busca o usuário pelo e-mail
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Ativo);

            // 2. Verifica se o usuário existe e se a senha bate
            // Importante: Se você usou Hash, aqui você compararia o Hash!
            if (usuario is null || usuario.Senha != loginDto.Senha)
            {
                // Regra de ouro: Mensagem genérica para segurança
                return Unauthorized("E-mail ou senha inválidos.");
            }

            // 3. Se deu tudo certo, por enquanto retornamos o usuário
            // No futuro, aqui você geraria um TOKEN JWT
            var resposta = new UsuarioResponseDto(
                usuario.Id,
                usuario.Nome!,
                usuario.Email!,
                usuario.DataCriacao,
                usuario.Ativo
            );

            return Ok(new { Mensagem = "Login realizado com sucesso!", Usuario = resposta });
        }
    }
}