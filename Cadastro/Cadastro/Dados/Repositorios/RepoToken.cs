using Cadastro.Intefaces;
using Cadastro.Modelos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cadastro.Dados.Repositorios;

// Adicione a herança da interface aqui!
public class RepoToken : ITokenRepositorio
{
    private readonly IConfiguration _config;

    public RepoToken(IConfiguration config)
    {
        _config = config;
    }

    public string GerarToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        // Buscando a chave do seu secrets.json
        var secret = _config["JwtSettings:Secret"];

        if (string.IsNullOrEmpty(secret))
            throw new Exception("A chave secreta do JWT não foi configurada no secrets.json!");

        var chave = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email.Valor), // Seu VO brilhando aqui!
                new Claim(ClaimTypes.Role, "UsuarioComum")
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(chave),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}