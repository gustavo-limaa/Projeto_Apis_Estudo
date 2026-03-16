using System.ComponentModel.DataAnnotations;

namespace FilmeApis.Data.Dtos;

public class UpadateEndereçoDto
{
   
    [Required]
    public string Locadouro { get; set; }
    [Required]
    public int Numero { get; set; }
}
