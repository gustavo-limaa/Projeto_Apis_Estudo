using Prateleira_Universal.Modelos.Enums;

namespace Prateleira_Universal.Dtos.ProdutoBaseDtos;

public record ProdutoBUpdate(

    string Nome,
    EnumCategoria Tipo,
    decimal Preco,
    string Descricao,
    Dictionary<string, string> Especificacoes
);