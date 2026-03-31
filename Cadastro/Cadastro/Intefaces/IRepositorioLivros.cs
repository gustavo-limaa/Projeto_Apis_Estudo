using Cadastro.Dtos.LivroDtos;
using Cadastro.Modelos;

namespace Cadastro.Intefaces;

public interface IRepositorioLivros
{
    // Usamos IReadOnlyList ou IEnumerable para listas
    Task<IEnumerable<Livro>> ObterTodosAsync();

    // Importante: Usar CancellationToken para permitir cancelar a busca (Boa Prática!)
    Task<Livro?> ObterPorIdAsync(Guid id);

    Task<bool> AdicionarAsync(Livro livro);

    // O Repositório recebe a entidade já pronta para salvar
    Task<bool> AtualizarAsync(Livro livro);

    Task<bool> DeletarAsync(Guid id);
}