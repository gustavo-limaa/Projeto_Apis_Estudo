using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.UsuarioCases
{
    public class ObterPorIdUsuarioUseCases
    {
        private readonly IRepositorioUsuario _repositorio;

        public ObterPorIdUsuarioUseCases(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<UsuarioResponseDto>> ExecutarAsync(Guid id)
        {
            if (id == Guid.Empty) return Result<UsuarioResponseDto>.Failure("ID do usuário é inválido.");

            var usuario = await _repositorio.ObterPorIdAsync(id);

            if (usuario is null)
            {
                return Result<UsuarioResponseDto>.Failure("Usuário não encontrado.");
            }
            return Result<UsuarioResponseDto>.Success(usuario.ToResponseDto());
        }
    }
}