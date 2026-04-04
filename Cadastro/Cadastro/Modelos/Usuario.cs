using Cadastro.Intefaces;
using System.ComponentModel.DataAnnotations;

namespace Cadastro.Modelos;

public class Usuario : EntidadeBase, IStatus
{
    public Usuario(Guid id, string? nome, string? senha, ValueEmail? email, bool ativo, ICollection<Livro> livros)
    {
        Id = id;
        Nome = nome;
        Senha = senha;
        Email = email;
        Ativo = ativo;
        Livros = livros;
    }

    public Usuario()
    {
    }

    [Required]
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Senha { get; set; }

    [Required, EmailAddress]
    public ValueEmail? Email { get; set; }

    [Required]
    public bool Ativo { get; set; } = true; public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();
}