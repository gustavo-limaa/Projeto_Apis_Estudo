using System.ComponentModel.DataAnnotations;

namespace FilmeApis.Data.Dtos;

public class ReadCinemaDto
{
    [Required]
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "o nome e necessario")]
    public string Name { get; set; }
    public ReadEnderecoDto  Endereco { get; set; }

    public ICollection<ReadSessaoDto> Sessoes { get; set; }

}
