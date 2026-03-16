using Microsoft.AspNetCore.Identity;

namespace UsuariosApi.Modelos;

public class Usuario : IdentityUser
{
    public DateTime DataNacimento { get; set; }
    public Usuario() : base()
    {

    }
}
