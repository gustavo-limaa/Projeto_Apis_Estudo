using Prateleira_Universal.Domain.Modelos.Enums;

namespace Prateleira_Universal.Domain.Modelos;

public class Produto : ProdutoBase
{
    public Produto()
    {
    }

    public Produto(Guid produtoID, string nome, EnumCategoria tipo, string descricao, decimal preco, Dictionary<string, string> espeficacoes) : base(produtoID, nome, tipo, descricao, preco, espeficacoes)
    {
    }
}