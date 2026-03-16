using System.ComponentModel.DataAnnotations;

namespace FilmeApis.Data.Dtos;

public class ReadEnderecoDto
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string Locadouro { get; set; }
    [Required]
    public int Numero { get; set; }
}
