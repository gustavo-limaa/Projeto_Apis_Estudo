namespace Prateleira_Universal.UseCases;

public class AtualizarProdutoUseCase
{
    private readonly IProdutoRepository _repo;

    public AtualizarProdutoUseCase(IProdutoRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> executarAsync(Guid id, ProdutoBUpdate DTO)
    {
        var produto = await _repo.ObterPorIdAsync(id);

        if (produto is null) return false;

        produto.UpdateFromDto(DTO);

        await _repo.AtualizarAsync(produto);

        return await _repo.SalvarAlteracoesAsync();
    }
}