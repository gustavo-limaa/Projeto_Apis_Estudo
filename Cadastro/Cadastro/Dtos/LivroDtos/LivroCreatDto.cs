using Cadastro.Modelos.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cadastro.Dtos.LivroDtos;

public record LivroCreatDto(
    [Required(ErrorMessage = "O Título é obrigatório")]
    [MaxLength(50, ErrorMessage = "O título não pode passar de 50 caracteres")]
    string Titulo,

    [Required(ErrorMessage = "O Autor é obrigatório")]
    string Autor,

    [Required(ErrorMessage = "O Preço é obrigatório")]
    [Range(0.01, 999999, ErrorMessage = "O preço mínimo é de 1 centavo")]
    decimal Preco,

    [Required(ErrorMessage = "A Descrição é obrigatória")]
    [MaxLength(300)]
    string? Descricao,

    [Required(ErrorMessage = "A Categoria é obrigatória")]
    CategoriaLivro? Categoria,

    [Required(ErrorMessage = "O ID do Usuário é obrigatório")]
    Guid UsuarioId
);