using Prateleira_Universal.Modelos.Enums;

namespace Prateleira_Universal.Dtos.ProdutoBaseDtos;

public record ProdutoBResponse(
    Guid ProdutoID, // O mesmo Guid da classe base!
    string Nome,
    string Descricao,
    EnumCategoria Tipo,
    decimal Preco,
    Dictionary<string, string> Especificacoes
);