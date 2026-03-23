using Prateleira_Universal.Domain.Modelos.Enums;

namespace Prateleira_Universal.UseCases.Dtos.ProdutoBaseDtos;

public record ProdutoBRequest
    (
    [Required][MaxLength(50)] string Nome,
    [Required][MaxLength(50)] string Descricao,
    [Required] EnumCategoria Tipo,
    Dictionary<string, string> Especificacoes,
   [Required][Range(0.01, 999999)] decimal Preco
    );