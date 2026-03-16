using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cadastro.Dtos.UsuarioDtos;

public record UsuarioCreatDto(
    [Required(ErrorMessage = "O nome é obrigatório")]
    string Nome,

    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido")]
    string Email,

    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "A senha deve conter ao menos uma letra maiúscula, uma minúscula, um número e um caractere especial.")]
    string Senha,

    [Property: Compare("Senha", ErrorMessage = "As senhas não conferem"  )]
    string ConfirmarSenha
);