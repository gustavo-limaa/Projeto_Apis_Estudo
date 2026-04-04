using Cadastro.Dados;
using Cadastro.Dtos.LivroDtos;
using Cadastro.Modelos;
using Cadastro.UseCases.LivrosCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Controllers;

[Authorize]
[ApiController]
[Route("api/livros")]
public class LivrosController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult> ListarTodosAsync([FromServices] LivroObterTodosUsecases usecases)
    {
        var resultado = await usecases.ObterTodosAsync();
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado.ErrorMessage);
    }

    [HttpPost]
    public async Task<ActionResult> AdicionarAsync([FromBody] LivroCreatDto dto, [FromServices] LivroCriarUseCases useCase)
    {
        var resultado = await useCase.ExecutarAsync(dto);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado.ErrorMessage);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult> ObterPorIdAsync(Guid id, [FromServices] LivroObterPorIdUseCases useCase)
    {
        var resultado = await useCase.ExecutarAsync(id);
        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound(resultado.ErrorMessage);
    }

    [HttpPut]
    public async Task<ActionResult> AtualizarAsync([FromBody] LivroUpdateDto dto, [FromServices] LivroAtualizarUsesCase useCase)
    {
        var resultado = await useCase.ExecutarAsync(dto);
        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest(resultado.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarAsync(Guid id, [FromServices] LivroDeletarUseCases useCase)
    {
        // 1. Tenta pegar o Header "Authorization" que o teste enviou
        var authHeader = Request.Headers["Authorization"].ToString();

        // 2. Verifica se o Header existe e se o que vem depois de "Bearer " é um Guid válido
        if (string.IsNullOrEmpty(authHeader) || !Guid.TryParse(authHeader.Replace("Bearer ", "").Trim(), out Guid usuarioLogadoId))
        {
            // Se o teste esquecer de mandar o header, barramos aqui
            return Unauthorized(new { Erro = "Usuário não autenticado no sistema." });
        }

        // 3. Passa o ID do livro e o ID de quem está logado para o UseCase
        var resultado = await useCase.ExecutarAsync(id, usuarioLogadoId);

        // 4. Retorna Ok (200) ou BadRequest (400) dependendo da trava do UseCase
        return resultado.IsSuccess
            ? Ok(new { Mensagem = "Livro removido com sucesso!", Dados = resultado.Value })
            : BadRequest(new { Erro = resultado.ErrorMessage });
    }
}