using Prateleira_Universal.interfaces;
using System.Net;

namespace Prateleira_Universal.Controllers;

[Route("api/produtos")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoRepository _repo;

    public ProdutoController(IProdutoRepository produtoRepository)
    {
        _repo = produtoRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CriarProdutoAsync(ProdutoBRequest request)
    {
        if (request is null) { return BadRequest("Dados inválidos"); }

        try
        {
            // 1. Criamos a entidade (o ID pode ser gerado no construtor da classe ou aqui)
            var novoid = Guid.NewGuid();
            var novoProduto = new Produto(
                novoid,
                request.Nome,
                request.Tipo,
                request.Descricao,
                request.Preco,
                request.Especificacoes
            );

            // 2. Usamos o REPOSITÓRIO para adicionar
            await _repo.AdicionarAsync(novoProduto);

            // 3. Persistimos através do REPOSITÓRIO
            var salvar = await _repo.SalvarAlteracoesAsync();

            if (!salvar) { return BadRequest("Não foi possível salvar o produto"); }
            // 4. Preparamos o DTO de resposta
            var response = new ProdutoBResponse(
                novoProduto.ProdutoID,
                novoProduto.Nome,
                novoProduto.Descricao,
                novoProduto.Tipo,
                novoProduto.Preco,
                novoProduto.Especificacoes
            );

            // 5. Retornamos o 201 Created com a rota de consulta
            return CreatedAtAction(
                nameof(ObterPorIdAsync), // Use o nome exato do seu método GET por ID
                new { id = novoProduto.ProdutoID },
                response
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

            if (produtos is null || !produtos.Any())
            {
                return NoContent(); // Retorna o Status 204
            }

            var response = produtos.Select(p => new ProdutoBResponse(
                p.ProdutoID,
                p.Nome,
                p.Descricao,
                p.Tipo,
                p.Preco,
                p.Especificacoes
            )).ToList();

            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocorreu um erro ao tentar recuperar os produtos");
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
            var response = new ProdutoBResponse(
                produto.ProdutoID,
                produto.Nome,
                produto.Descricao,
                produto.Tipo,
                produto.Preco,
                produto.Especificacoes
            );

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
}