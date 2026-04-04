using Cadastro.Dtos.LoginDtos;
using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;

namespace Cadastro.UseCases.LoginCases;

public class LoginUsuarioUseCase
{
    private readonly IRepositorioUsuario _repoUsuario;
    private readonly ITokenRepositorio _tokenRepo;

    public LoginUsuarioUseCase(IRepositorioUsuario repoUsuario, ITokenRepositorio tokenRepo)
    {
        _repoUsuario = repoUsuario;
        _tokenRepo = tokenRepo;
    }

    public async Task<Result<LoginResponseDto>> ExecutarAsync(LoginDto loginData)
    {
        // 1. Busca o usuário pelo VO de Email (assumindo que seu repo aceita string ou converte)
        var usuario = await _repoUsuario.ObterPorEmailAsync(loginData.Email);

        if (usuario == null)
            return Result<LoginResponseDto>.Failure("E-mail ou senha incorretos.");

        // 2. Validação de Senha (IMPORTANTE: Use BCrypt aqui se as senhas estiverem hasheadas)
        // Se ainda estiver em texto puro para teste: if (usuario.Senha != loginData.Senha)
        if (!BCrypt.Net.BCrypt.Verify(loginData.Senha, usuario.Senha))
        {
            return Result<LoginResponseDto>.Failure("E-mail ou senha incorretos.");
        }

        // 3. Gera o Token usando o seu RepoToken
        var token = _tokenRepo.GerarToken(usuario);

        // 4. Retorna o DTO de resposta
        return Result<LoginResponseDto>.Success(new LoginResponseDto(
            usuario.Email.Valor,
            token,
            "Login realizado com sucesso!"
        ));
    }
}