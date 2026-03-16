using System.ComponentModel.DataAnnotations;

namespace FilmeApis.Data.Dtos;

public class UpdateCinemaDto
{
    [Required(ErrorMessage = "o nome e necessario")]
    public string Name { get; set; }
}
