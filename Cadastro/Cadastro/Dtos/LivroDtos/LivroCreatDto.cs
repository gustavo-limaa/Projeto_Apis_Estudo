namespace Cadastro.Dtos.LivroDtos;

public record LivroCreatDto(string Titulo,
    string Autor,
    decimal Preco,
    string? Descricao,
    Guid UsuarioId);