using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace Cadastro.Modelos;
// "partial" é necessário para o GeneratedRegex

public partial record ValueEmail
{
    public string Valor { get; }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex(); // Mude para PRIVATE

    public ValueEmail(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor) || !EmailRegex().IsMatch(valor))
            throw new ArgumentException("E-mail inválido.");

        Valor = valor.ToLower().Trim();
    }
}