using System.Linq;
using Cadastro.Dtos.LoginDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.ValueObjects;
using Cadastro.UseCases.LoginCases.Validacoes;
using FluentValidation;

namespace Cadastro.UseCases.LoginCases
{
    public class RefreshTokenUsesCases
    {
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly ITokenRepositorio _tokenRepo;
        private readonly IValidator<RefreshTokenRequestDto> _validator;

        public RefreshTokenUsesCases(IRepositorioUsuario repoUsuario, ITokenRepositorio tokenRepo, IValidator<RefreshTokenRequestDto> validator)
        {
            _repoUsuario = repoUsuario;
            _tokenRepo = tokenRepo;
            _validator = validator;
        }

        public async Task<Result<LoginResponseDto>> ExecutarAsync(RefreshTokenRequestDto dto)
        {
            // 1. PRIMEIRO: Valida o DTO (Não gasta banco se o token vier vazio)
            var validacao = await _validator.ValidateAsync(dto);

            if (!validacao.IsValid)
                return Result<LoginResponseDto>.Failure(validacao.Errors.First().ErrorMessage);

            // 2. SEGUNDO: Agora que o DTO está ok, busca no banco
            var usuario = await _repoUsuario.ObterPorRefreshTokenAsync(dto.RefreshToken);

            // 3. TERCEIRO: Validações de Negócio
            if (usuario == null)
                return Result<LoginResponseDto>.Failure("Token inválido.");

            // Como RefreshToken é um Value Object, cuidado com o Null Check se o banco trouxer nulo
            if (usuario.RefreshToken == null || usuario.RefreshToken.EstaExpirado)
                return Result<LoginResponseDto>.Failure("Sessão expirada. Faça login novamente.");

            if (!usuario.Ativo)
                return Result<LoginResponseDto>.Failure("Usuário inativo.");

            // 4. ROTAÇÃO: Gerar tudo novo
            var novoJwt = _tokenRepo.GerarToken(usuario);
            var novoRefreshToken = RefreshTokenValueObject.GerarNovo(7);

            // 5. Salva o novo estado no banco
            usuario.AtualizarRefreshToken(novoRefreshToken);
            await _repoUsuario.SalvarAlteracoesAsync(usuario);

            return Result<LoginResponseDto>.Success(new LoginResponseDto(
                usuario.Email.Valor,
                novoJwt,
                novoRefreshToken.Token,
                "Token renovado com sucesso!"
            ));
        }
    }
}