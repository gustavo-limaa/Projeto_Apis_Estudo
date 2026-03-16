using Cadastro.Dados;
using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var usuarios = await _context.Usuarios.ToListAsync();

            if (usuarios is not null && usuarios.Any())
            {
                var resposta = usuarios.Select(u => new UsuarioResponseDto
                (
                   u.Id,
                   u.Nome!,
                   u.Email!,
                   u.DataCriacao,
                   u.Ativo

                )).ToList();

                return Ok(resposta);
            }

            return NotFound("Usuario nao encontrado");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario is not null)
            {
                var resposta = new UsuarioResponseDto(
                    usuario.Id,
                    usuario.Nome!,
                    usuario.Email!,
                    usuario.DataCriacao,
                    usuario.Ativo
                );
                return Ok(resposta);
            }
            return NotFound("Usuario nao encontrado");
        }

        [HttpPost]
        public async Task<IActionResult> Criar(UsuarioCreatDto usuarioDto)
        {
            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                Senha = usuarioDto.Senha,

                Ativo = true
            };
            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            var resposta = new UsuarioResponseDto(
                usuario.Id,
                usuario.Nome!,
                usuario.Email!,
                usuario.DataCriacao,
                usuario.Ativo
            );
            return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, resposta);
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar(UsuarioUpdateDto usuarioDto)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioDto.Id);

            if (usuario is not null)
            {
                usuario.Nome = usuarioDto.Nome;
                usuario.Email = usuarioDto.Email;
                usuario.Ativo = usuarioDto.Ativo;

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound("Usuario nao encontrado");
        }

        [HttpDelete]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is not null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound("Usuario nao encontrado");
        }
    }
}