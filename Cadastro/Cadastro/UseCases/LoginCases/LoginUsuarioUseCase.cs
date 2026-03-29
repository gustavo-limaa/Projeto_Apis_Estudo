using Cadastro.Dtos.LoginDtos;
using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.LoginCases
{
    public class LoginUsuarioUseCase
    {
        private readonly IRepositorioUsuario _repositorioUsuario;

        public LoginUsuarioUseCase(IRepositorioUsuario repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario;
        }

        public async Task<Result<UsuarioResponseDto>> ExecutarAsync(LoginDto dto)
        {
            // 1. Usamos o Repositorio de USUÁRIO para buscar o dono do e-mail
            var usuario = await _repositorioUsuario.ObterPorEmailAsync(dto.Email);

            // 2. Se o usuário não existe ou a senha não bate...
            if (usuario == null || usuario.Senha != dto.Senha)
            {
                return Result<UsuarioResponseDto>.Failure("E-mail ou senha inválidos.");
            }

            // 3. Se deu certo, o 'usuario' (Entidade) vira 'ResponseDto' (Saída)
            return Result<UsuarioResponseDto>.Success(usuario.ToResponseDto());
        }
    }
}