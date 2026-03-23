using Prateleira_Universal.Domain.Modelos.Enums;

namespace Prateleira_Universal.UseCases.Dtos.ProdutoBaseDtos;

public record ProdutoBUpdate(

  [Required][MaxLength(50)] string Nome,
  [Required] EnumCategoria Tipo,
 [Required][Range(0.01, 999999)] decimal Preco,
   [Required][MaxLength(50)] string Descricao,
    Dictionary<string, string> Especificacoes
);