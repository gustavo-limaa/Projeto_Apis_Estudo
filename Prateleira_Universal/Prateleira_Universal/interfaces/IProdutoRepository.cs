namespace Prateleira_Universal.interfaces;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> ObterTodosAsync();

    Task<Produto?> ObterPorIdAsync(Guid id);

    Task AdicionarAsync(Produto produto);

    Task AtualizarAsync(Produto produto);

    Task DeletarAsync(Produto produto);

    Task<bool> SalvarAlteracoesAsync();
}