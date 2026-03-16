using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using UsuariosApi.Modelos; // Adicione este using

public class TokenService
{
    private readonly IConfiguration _configuration;

    // Construtor para receber as configurações do sistema
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Usuario usuario)
    {
        var claims = new Claim[]
        {
            new Claim("username", usuario.UserName),
            new Claim("id", usuario.Id.ToString()),
            new Claim(ClaimTypes.DateOfBirth, usuario.DataNacimento.ToString())
        };

        // Busca a chave que você salvou no terminal via dotnet user-secrets
        var chaveKey = _configuration["JwtKey"];
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveKey));
        var signingCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        // Corrigido: Aqui usamos parênteses ( ) para o construtor, não chaves { }
        var tokenDescriptor = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddMinutes(10),
            claims: claims,
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenDescriptor);
    }
}