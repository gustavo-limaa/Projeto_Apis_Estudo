using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Prateleira_Universal.Domain.interfaces;
using Prateleira_Universal.interfaces;
using Prateleira_Universal.UseCases;
using Prateleira_Universal.UseCases.Dtos.ProdutoBaseDtos;
using Prateleira_Universal.UseCases.Mappers;
using Refit;
using System.Net;

namespace Prateleira_Universal.Controllers;

[Route("api/produtos")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoRepository _repo;
    private readonly CriarProdutoUseCase _criarProdutoUseCase;

    public ProdutoController(IProdutoRepository produtoRepository, CriarProdutoUseCase criarProdutoUseCase)
    {
        _repo = produtoRepository;
        _criarProdutoUseCase = criarProdutoUseCase;
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
            var produtos = await _repo.ObterTodosAsync();

            // Se 'produtos' for vazio, o Select simplesmente não roda nenhuma vez
            // e o 'response' já nasce como uma lista vazia.
            var response = produtos.Select(p => p.ToResponse()).ToList();

            return Ok(response);
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
            var produto = await _repo.ObterPorIdAsync(id);

            if (produto is null)
            {
                return NotFound();
            }
            // Transformamos a entidade no DTO de resposta
            var response = produto.ToResponse();

            return Ok(response);
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
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var produto = await _repo.ObterPorIdAsync(id);

            if (produto is null) { return NotFound(); }

            await _repo.DeletarAsync(produto);

            var salvar = await _repo.SalvarAlteracoesAsync();

            if (!salvar) { return BadRequest(); }

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao tentar deletar");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> AtualizarProdutoAsync(Guid id, [FromBody] ProdutoBUpdate request)
    {
        try
        {
            if (request is null) { return BadRequest(); }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var produto = await _repo.ObterPorIdAsync(id);

            if (produto is null) { return NotFound(); }

            produto.Nome = request.Nome;
            produto.Descricao = request.Descricao;
            produto.Tipo = request.Tipo;
            produto.Preco = request.Preco;
            produto.Especificacoes = request.Especificacoes;

            await _repo.AtualizarAsync(produto);
            var salvar = await _repo.SalvarAlteracoesAsync();
            if (!salvar) { return BadRequest(); }

            return NoContent(); // Status 204
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao tentar atualizar o produto");
        }
    }

    [HttpGet("categoria/{categoria}")] // Adicione a rota para o Refit achar
    public async Task<ActionResult<IEnumerable<ProdutoBResponse>>> ObterPorCategoria(EnumCategoria categoria)
    {
        try
        {
            // 1. Valida se o Enum existe (Segurança)
            if (!Enum.IsDefined(typeof(EnumCategoria), categoria))
            {
                return BadRequest("Essa categoria de produto não existe no nosso catálogo.");
            }

            // 2. Busca no Repositório
            var buscar = await _repo.ObterPorCategoriaAsync(categoria);

            // 3. Converte para DTO usando o ctor que criamos
            // Adicionamos os () no ToList()!
            var response = buscar.Select(p => p.ToResponse()).ToList();

            // 4. Se a lista estiver vazia, o 'response' será [] e o status será 200 OK
            return Ok(response);
        }
        catch (Exception)
        {
            // Logar o erro 'ex' seria o ideal aqui no futuro!
            return StatusCode(500, "Erro interno ao buscar categoria.");
        }
    }
}