using Prateleira_Universal.Domain.Modelos.Enums;
using System.ComponentModel.DataAnnotations;

namespace Prateleira_Universal.Domain.Modelos;

public abstract class ProdutoBase
{
    [Key]
    public Guid ProdutoID { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    public EnumCategoria Tipo { get; set; }

    [Required]
    [MaxLength(100)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public decimal Preco { get; set; }

    public Dictionary<string, string> Especificacoes { get; set; }

    public ProdutoBase(Guid produtoID, string nome, EnumCategoria tipo, string descricao, decimal preco, Dictionary<string, string> espeficacoes)
    {
        ProdutoID = produtoID;
        Nome = nome;
        Tipo = tipo;
        Descricao = descricao;
        Preco = preco;
        Especificacoes = espeficacoes;
    }

    public ProdutoBase()
    {
    }
}