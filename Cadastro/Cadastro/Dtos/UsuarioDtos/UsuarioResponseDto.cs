namespace Cadastro.Dtos.UsuarioDtos;

public record UsuarioResponseDto(
    Guid Id,
    string Nome,
    string Email,
    DateTime DataCriacao,
    bool Ativo
);