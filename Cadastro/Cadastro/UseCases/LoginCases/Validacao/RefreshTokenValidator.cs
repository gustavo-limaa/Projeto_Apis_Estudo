using FluentValidation;
using Cadastro.Dtos.LoginDtos;

namespace Cadastro.UseCases.LoginCases.Validacoes;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("O token não pode estar em branco.")
            .NotNull().WithMessage("O envio do token é obrigatório.");
    }
}