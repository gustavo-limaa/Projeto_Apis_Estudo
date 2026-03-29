using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.UsuarioCases
{
    public class ListarTodosUsuarioUseCases
    {
        private readonly IRepositorioUsuario _repositorio;

        public ListarTodosUsuarioUseCases(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<IEnumerable<UsuarioResponseDto>>> ExecutarAsync()
        {
            var usuarios = await _repositorio.ListartodosAsync();

            if (usuarios == null)
            {
                return Result<IEnumerable<UsuarioResponseDto>>.Failure("Nenhum usuário encontrado.");
            }
            return Result<IEnumerable<UsuarioResponseDto>>.Success(usuarios.Select(u => u.ToResponseDto()).ToList());
        }
    }
}