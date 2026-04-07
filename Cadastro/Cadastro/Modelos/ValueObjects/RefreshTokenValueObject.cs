namespace Cadastro.Modelos.ValueObjects;

public class RefreshTokenValueObject
{
    public string Token { get; }
    public DateTime DataExpiracao { get; }

    public RefreshTokenValueObject(string token, DateTime dataExpiracao)
    {
        if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException("Token inválido");
        Token = token;
        DataExpiracao = dataExpiracao;
    }

    public bool EstaExpirado => DateTime.UtcNow >= DataExpiracao;

    // Método para criar um novo de forma fácil
    public static RefreshTokenValueObject GerarNovo(int diasValidade = 7)
    {
        return new RefreshTokenValueObject(
            Guid.NewGuid().ToString().Replace("-", ""),
            DateTime.UtcNow.AddDays(diasValidade)
        );
    }
}