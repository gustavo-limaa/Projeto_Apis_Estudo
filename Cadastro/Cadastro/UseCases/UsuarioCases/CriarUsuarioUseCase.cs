using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.UsuarioCases
{
    public class CriarUsuarioUseCase
    {
        private readonly IRepositorioUsuario _repositorio;

        public CriarUsuarioUseCase(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<UsuarioResponseDto>> ExecutarAsync(UsuarioCreatDto dto)
        {
            if (dto.Senha != dto.ConfirmarSenha)
                return Result<UsuarioResponseDto>.Failure("As senhas não coincidem.");

            try
            {
                var usuario = dto.ToEntity();

                var usuarioSalvo = await _repositorio.AdicionarAsync(usuario);

                if (usuarioSalvo == null)
                    return Result<UsuarioResponseDto>.Failure("Erro ao salvar o usuário.");

                return Result<UsuarioResponseDto>.Success(usuarioSalvo.ToResponseDto());
            }
            catch (Exception ex)
            {
                // Aqui pegamos o erro do ValueObject (ex: "E-mail inválido")
                return Result<UsuarioResponseDto>.Failure(ex.Message);
            }
        }
    }
}