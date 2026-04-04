using Cadastro.Intefaces;
using Cadastro.Modelos.Enums;
using Cadastro.Modelos.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastro.Modelos;

public class Livro : EntidadeBase, IStatus
{
    public Livro(string? titulo, string? autor, ValorMonetario preco, string? descricao, CategoriaLivro categoria, Guid usuarioId, bool ativo, Usuario? usuario)
    {
        Titulo = titulo;
        Autor = autor;
        Preco = preco;
        Descricao = descricao;
        Categoria = categoria;
        UsuarioId = usuarioId;
        Ativo = ativo;
        Usuario = usuario;
    }

    public Livro()
    {
    }

    [Required]
    public string? Titulo { get; set; }

    [Required]
    public string? Autor { get; set; }

    [Required]
    public ValorMonetario Preco { get; set; } = new();

    [Required]
    public string? Descricao { get; set; }

    // Adição da Categoria
    [Required]
    public CategoriaLivro Categoria { get; set; }

    [ForeignKey("UsuarioId")]
    public Guid UsuarioId { get; set; }

    public bool Ativo { get; set; } = true;

    public virtual Usuario? Usuario { get; set; }
}