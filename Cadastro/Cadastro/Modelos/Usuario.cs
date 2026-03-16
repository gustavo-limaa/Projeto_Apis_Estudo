using Cadastro.Intefaces;
using System.ComponentModel.DataAnnotations;

namespace Cadastro.Modelos;

public class Usuario : EntidadeBase, IStatus
{
    [Required]
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Senha { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    public bool Ativo { get; set; } = true;
    public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();
}