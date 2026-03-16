using System.ComponentModel.DataAnnotations;

namespace Cadastro.Dtos.LoginDtos;

public record LoginDto(
    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress]
    string Email,

    [Required(ErrorMessage = "A senha é obrigatória")]
    string Senha
);