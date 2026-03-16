using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace FilmeApis.Models;

public class Sessao
{
    [Key]
    [Required]
    public int Id { get; set; }

    // Chaves Estrangeiras para relacionar as tabelas
    public int FilmeId { get; set; }
    public int CinemaId { get; set; }

    // Propriedades de navegação para o Entity Framework carregar os dados
    [Required]
    public virtual Filme Filme { get; set; }
    [Required]
    public virtual Cinema Cinema { get; set; }
}