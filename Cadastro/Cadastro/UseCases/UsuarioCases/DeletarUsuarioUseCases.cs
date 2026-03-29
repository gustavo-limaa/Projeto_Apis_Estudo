using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.UsuarioCases
{
    public class DeletarUsuarioUseCases
    {
        private readonly IRepositorioUsuario _repositorio;

        public DeletarUsuarioUseCases(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<bool>> ExecutarAsync(Guid id)
        {
            if (id == Guid.Empty) Result<bool>.Failure("ID do usuário é inválido.");

            // O seu repositório já tem a lógica de buscar e depois dar o ExecuteDeleteAsync
            var usuarioDeletado = await _repositorio.DeletarAsync(id);

            if (usuarioDeletado == null) return Result<bool>.Failure("Usuário não encontrado para deleção.");
            // Se o repositório retornar o objeto que foi deletado, significa que deu certo!
            return Result<bool>.Success(true);
        }
    }
}