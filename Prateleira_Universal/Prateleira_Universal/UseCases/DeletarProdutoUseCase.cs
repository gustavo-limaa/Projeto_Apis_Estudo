namespace Prateleira_Universal.UseCases;

public class DeletarProdutoUseCase
{
    private readonly IProdutoRepository _Repo;

    public DeletarProdutoUseCase(IProdutoRepository repo)
    {
        _Repo = repo;
    }

    public async Task ExecutarAsync(Guid id)
    {
        // 1. Fail First: Se não existe, para aqui.
        var produto = await _Repo.ObterPorIdAsync(id);

        if (produto is null)
        {
            // Aqui você pode lançar uma exception customizada ou retornar um Result.
            throw new KeyNotFoundException("Produto não encontrado.");
        }

        // 2. Ação Direta: Deleta o objeto recuperado
        await _Repo.DeletarAsync(produto);

        // 3. Persistência
        await _Repo.SalvarAlteracoesAsync();
    }
}