using Prateleira_Universal.Modelos.Enums;

namespace Prateleira_Universal.Dtos.ProdutoBaseDtos;

public record ProdutoBRequest
    (string Nome,
    string Descricao,
    EnumCategoria Tipo,
    Dictionary<string, string> Especificacoes,
    decimal Preco
    );