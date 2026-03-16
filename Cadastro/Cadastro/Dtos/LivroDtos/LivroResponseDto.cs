namespace Cadastro.Dtos.LivroDtos;

record LivroResponseDto(Guid Id,
    string Titulo,
    string Autor,
    decimal Preco,
    string? Descricao,
    DateTime Datacricao,
    bool Ativo);