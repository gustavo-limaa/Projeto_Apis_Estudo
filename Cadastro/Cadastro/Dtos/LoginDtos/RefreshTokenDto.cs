using System.ComponentModel.DataAnnotations;

namespace Cadastro.Dtos.LoginDtos;

public record RefreshTokenRequestDto
(
    [Required(ErrorMessage = "Campo Obrigatorio")] string RefreshToken
);