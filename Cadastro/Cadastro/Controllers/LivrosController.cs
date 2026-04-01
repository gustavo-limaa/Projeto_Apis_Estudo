using Cadastro.Dados;
using Cadastro.Dtos.LivroDtos;
using Cadastro.Modelos;
using Cadastro.UseCases.LivrosCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Controllers;

[ApiController]
[Route("api/livros")]
public class LivrosController : ControllerBase
{
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
        var resultado = await useCase.ExecutarAsync(id);

        // Em vez de NoContent (204), usamos Ok (200) enviando o DTO que você buscou no UseCase
        return resultado.IsSuccess
            ? Ok(new { Mensagem = "Livro removido com sucesso!", Dados = resultado.Value })
            : NotFound(new { Erro = resultado.ErrorMessage });
    }
}