using Prateleira_Universal.Domain.Modelos.Enums;

namespace Prateleira_Universal.UseCases.Dtos.ProdutoBaseDtos;

public record ProdutoBResponse(
    Guid ProdutoID, // O mesmo Guid da classe base!
  [Required][MaxLength(50)] string Nome,
   [Required][MaxLength(50)] string Descricao,
   [Required] EnumCategoria Tipo,
 [Required][Range(0.01, 999999)] decimal Preco,
    Dictionary<string, string> Especificacoes
);