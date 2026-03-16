using System.ComponentModel.DataAnnotations;

namespace Cadastro.Modelos;

public class EntidadeBase
{
    [Required]
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid(); // Para praticar o uso de GUIDs

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow; // Para praticar manipulação de datas
}