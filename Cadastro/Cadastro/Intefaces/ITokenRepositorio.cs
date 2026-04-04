using Cadastro.Modelos;

namespace Cadastro.Intefaces
{
    public interface ITokenRepositorio
    {
        string GerarToken(Usuario usuario);
    }
}