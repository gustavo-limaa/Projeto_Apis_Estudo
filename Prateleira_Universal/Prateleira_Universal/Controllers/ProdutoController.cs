using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Prateleira_Universal.Domain.interfaces;
using Prateleira_Universal.interfaces;
using Prateleira_Universal.UseCases;
using Prateleira_Universal.UseCases.Dtos.ProdutoBaseDtos;
using Refit;
using System.Net;

namespace Prateleira_Universal.Controllers;

[Route("api/produtos")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly CriarProdutoUseCase _criarProdutoUseCase;
    private readonly ObterProdutoPorIdUseCase _obterProdutoPorIdUseCase;
    private readonly ObterProdutosPorCategoriaUseCase _obterProdutosPorCategoriaUseCases;
    private readonly ObterTodosProdutosUseCase _obterTodosProdutosUseCase;
    private readonly AtualizarProdutoUseCase _atualizarProdutoUseCase;
    private readonly DeletarProdutoUseCase _deletarProdutoUseCase;

    public ProdutoController(CriarProdutoUseCase criarProdutoUseCase, ObterProdutoPorIdUseCase obterProdutoPorIdUseCase, ObterProdutosPorCategoriaUseCase obterProdutosPorCategoriaUseCases, ObterTodosProdutosUseCase obterTodosProdutosUseCase, AtualizarProdutoUseCase atualizarProdutoUseCase, DeletarProdutoUseCase deletarProdutoUseCase)
    {
        _criarProdutoUseCase = criarProdutoUseCase;
        _obterProdutoPorIdUseCase = obterProdutoPorIdUseCase;
        _obterProdutosPorCategoriaUseCases = obterProdutosPorCategoriaUseCases;
        _obterTodosProdutosUseCase = obterTodosProdutosUseCase;
        _atualizarProdutoUseCase = atualizarProdutoUseCase;
        _deletarProdutoUseCase = deletarProdutoUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> CriarProdutoAsync(ProdutoBRequest request)
    {
        if (request is null) { return BadRequest("Dados inválidos"); }

        try
        {
            var reponse = await _criarProdutoUseCase.ExecutarAsync(request);

            return CreatedAtAction(
                nameof(ObterPorIdAsync), // Use o nome exato do seu método GET por ID
                new { id = reponse.ProdutoID },
                reponse
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message + " | " + ex.InnerException?.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoBResponse>>> ObterTodosAsync()
    {
        try
        {
            var produtos = await _obterTodosProdutosUseCase.ExecutarAsync();

            return Ok(produtos);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao recuperar produtos");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoBResponse>> ObterPorIdAsync(Guid id)
    {
        try
        {
            var produto = await _obterProdutoPorIdUseCase.ExecutarAsync(id);

            if (produto is null)
            {
                return NotFound();
            }
            return Ok(produto);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocorreu um erro ao tentar recuperar o produto");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarProdutoAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty) return BadRequest("ID inválido");

            // O UseCase cuida de buscar, validar e deletar
            await _deletarProdutoUseCase.ExecutarAsync(id);

            return NoContent(); // 204 Sucesso sem conteúdo
        }
        catch (KeyNotFoundException) // Se o UseCase não achar o produto
        {
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao tentar deletar");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> AtualizarProdutoAsync(Guid id, [FromBody] ProdutoBUpdate request)
    {
        // Validação básica de entrada
        if (id == Guid.Empty || request is null) return BadRequest("Dados inválidos.");

        try
        {
            // O Controller apenas dá a ordem
            var produto = await _atualizarProdutoUseCase.executarAsync(id, request);

            return NoContent(); // 204 Sucesso
        }
        catch (KeyNotFoundException)
        {
            // Se o UseCase não achar o produto, ele lança essa exceção e o Controller retorna 404
            return NotFound("Produto não encontrado.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao tentar atualizar: {ex.Message}");
        }
    }

    [HttpGet("categoria/{categoria}")] // Adicione a rota para o Refit achar
    public async Task<ActionResult<IEnumerable<ProdutoBResponse>>> ObterPorCategoria(EnumCategoria categoria)
    {
        if (!Enum.IsDefined(typeof(EnumCategoria), categoria))
        {
            return BadRequest("Essa categoria de produto não existe no nosso catálogo.");
        }

        try
        {
            var reponse = await _obterProdutosPorCategoriaUseCases.ExecutarAsync(categoria);

            return Ok(reponse);
        }
        catch (ArgumentException ex) // Se a categoria for inválida
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            // Logar o erro 'ex' seria o ideal aqui no futuro!
            return StatusCode(500, "Erro interno ao buscar categoria.");
        }
    }
}