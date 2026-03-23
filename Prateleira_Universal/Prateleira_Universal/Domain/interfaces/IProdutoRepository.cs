using Prateleira_Universal.Domain.Modelos;
using Prateleira_Universal.Domain.Modelos.Enums;

namespace Prateleira_Universal.Domain.interfaces;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> ObterTodosAsync();

    Task<Produto?> ObterPorIdAsync(Guid id);

    Task AdicionarAsync(Produto produto);

    Task AtualizarAsync(Produto produto);

    Task DeletarAsync(Produto produto);

    Task<IEnumerable<Produto>> ObterPorCategoriaAsync(EnumCategoria categoria);

    Task<bool> SalvarAlteracoesAsync();
}