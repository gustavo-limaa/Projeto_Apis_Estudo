using Cadastro.Dados;
using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Modelos;
using Cadastro.UseCases.UsuarioCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        // Injetamos apenas os Casos de Uso que vamos usar
        private readonly CriarUsuarioUseCase _criarUseCase;

        private readonly ListarTodosUsuarioUseCases _listarTodosUsuarioUseCases;
        private readonly DeletarUsuarioUseCases _deletarUsuarioUseCases;
        private readonly AtualizarUsuarioUseCases _atualizarUseCase;
        private readonly ObterPorIdUsuarioUseCases _obterUseCase;

        public UsuarioController(
            CriarUsuarioUseCase criarUseCase,
            AtualizarUsuarioUseCases atualizarUseCase,
            ObterPorIdUsuarioUseCases obterUseCase,
            ListarTodosUsuarioUseCases listarTodosUsuarioUseCases,
            DeletarUsuarioUseCases deletarUsuarioUseCases)
        {
            _criarUseCase = criarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _obterUseCase = obterUseCase;
            _listarTodosUsuarioUseCases = listarTodosUsuarioUseCases;
            _deletarUsuarioUseCases = deletarUsuarioUseCases;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { Message = "Id inválido." });
            }
            // O UseCase resolve a busca e a conversão para DTO
            var resultado = await _obterUseCase.ExecutarAsync(id);

            if (!resultado.IsSuccess)
            {
                return NotFound(new { Message = resultado.ErrorMessage });
            }

            return Ok(resultado.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioUpdateDto dto)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { Message = "Id inválido." });
            }
            // Se o e-mail no DTO for inválido, o UseCase (via Value Object) estoura o erro aqui!
            var resultado = await _atualizarUseCase.ExecutarAsync(id, dto);

            if (!resultado.IsSuccess)
            {
                return NotFound(new { Message = resultado.ErrorMessage });
            }
            return Ok(resultado.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] UsuarioCreatDto dto)
        {
            // 1. Chama o UseCase
            var resultado = await _criarUseCase.ExecutarAsync(dto);

            // 2. Se o envelope diz que falhou, retorna o erro amigável
            if (!resultado.IsSuccess)
            {
                return BadRequest(new { Message = resultado.ErrorMessage });
            }

            // 3. O sucesso! Pegamos o ID de dentro do '.Value' (que é um UsuarioResponseDto)
            return CreatedAtAction(
                actionName: nameof(ObterPorId),
                routeValues: new { id = resultado.Value.Id }, // AQUI ESTÁ O ID!
                value: resultado.Value // Retorna o DTO completo para o usuário
            );
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var resultado = await _listarTodosUsuarioUseCases.ExecutarAsync();

            if (!resultado.IsSuccess)
            {
                return NotFound(new { Message = resultado.ErrorMessage });
            }

            return Ok(resultado.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { Message = "Id inválido." });
            }

            var resultado = await _deletarUsuarioUseCases.ExecutarAsync(id);
            if (!resultado.IsSuccess)
            {
                return NotFound(new { Message = resultado.ErrorMessage });
            }
            return NoContent();
        }
    }
}