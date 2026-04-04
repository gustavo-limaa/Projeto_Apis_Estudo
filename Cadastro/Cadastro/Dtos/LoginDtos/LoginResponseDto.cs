namespace Cadastro.Dtos.LoginDtos;

public record LoginResponseDto(
    string Email,
    string Token,
    string Mensagem);