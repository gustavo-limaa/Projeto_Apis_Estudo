using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Modelos;

namespace Cadastro.Intefaces
{
    public interface IRepositorioUsuario
    {
        Task<IEnumerable<Usuario>> ListartodosAsync();

        Task<Usuario> ObterPorIdAsync(Guid id);

        Task<Usuario> AdicionarAsync(Usuario usuario);

        Task<Usuario> AtualizarAsync(Guid id, UsuarioUpdateDto dto);

        Task<Usuario> DeletarAsync(Guid id);

        Task<Usuario?> ObterPorEmailAsync(string email);
    }
}