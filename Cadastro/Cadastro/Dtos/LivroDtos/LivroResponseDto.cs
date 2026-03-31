using System.ComponentModel.DataAnnotations;

namespace Cadastro.Dtos.LivroDtos;

public record LivroResponseDto(

    Guid Id,
    string Titulo,
    string Autor,
    decimal Preco,

    string? Descricao,

    string Categoria,

    DateTime Datacricao,
    bool Ativo);