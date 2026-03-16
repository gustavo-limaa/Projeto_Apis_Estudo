using System.ComponentModel.DataAnnotations;
namespace FilmeApis.Models;


public class Cinema
{
    [Required]
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "o nome e necessario")]
    public string Name { get; set; }

    public int EnderecoId { get; set; }
    [Required]
    public virtual Endereco Endereco {  get; set; }
    [Required]
    public virtual ICollection<Sessao> Sessoes { get; set; }
}
