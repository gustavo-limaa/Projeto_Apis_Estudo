namespace Cadastro.Dtos.LivroDtos;

public record LivroUpdateDto(Guid Id,
    string Titulo,
    string Autor,
    decimal Preco,
    string? Descricao,
    bool Ativo
    );