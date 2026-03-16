using Cadastro.Intefaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastro.Modelos;

public class Livro : EntidadeBase, IStatus
{
    [Required]
    public string? Titulo { get; set; }

    [Required]
    public string? Autor { get; set; }

    [Required]
    public decimal Preco { get; set; } // Para praticar tipos numéricos

    public string? Descricao { get; set; }

    [ForeignKey("UsuarioId")]
    public Guid UsuarioId { get; set; } // Para praticar relacionamentos entre entidades

    public bool Ativo { get; set; } = true;

    [ForeignKey("UsuarioId")]
    public virtual Usuario Usuario { get; set; }
}