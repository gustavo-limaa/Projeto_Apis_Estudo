using System.ComponentModel.DataAnnotations;

namespace Cadastro.Dtos.LoginDtos;

public record LoginResponseDto(
   [Required] string Email,
   [Required] string Token,
  [Required] string refreshToken,
 [Required] string Mensagem);