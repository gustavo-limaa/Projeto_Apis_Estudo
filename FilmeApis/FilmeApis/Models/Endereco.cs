using System.ComponentModel.DataAnnotations;

namespace FilmeApis.Models
{
    public class Endereco
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Locadouro { get; set; }
        [Required]
        public int Numero { get; set; }
        public virtual Cinema Cinema { get; set; }
    }
}
