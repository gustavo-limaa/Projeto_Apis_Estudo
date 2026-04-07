using System.Linq;
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
            if (usuarios == null || !usuarios.Any())
            {
                return Result<IEnumerable<UsuarioResponseDto>>.Failure("Banco Vazio");
            }

            var usuariosDto = usuarios.Select(u => u.ToResponseDto()).ToList();
            return Result<IEnumerable<UsuarioResponseDto>>.Success(usuariosDto);
        }
    }
}