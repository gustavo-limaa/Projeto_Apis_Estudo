using System.ComponentModel.DataAnnotations;

namespace FilmeApis.Data.Dtos;

public class CreatCinemaDto
{
    [Required(ErrorMessage = "o nome e necessario")]
    public string Name { get; set; }
    [Required]
    public int EndereçoID { get; set; }



}
