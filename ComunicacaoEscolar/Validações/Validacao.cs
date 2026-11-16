

using System.Text.RegularExpressions;
 
namespace ComunicacaoEscolar.Validacoes;
 
public static class Validador

{

    private static readonly Regex EmailRegex = new(

        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",

        RegexOptions.Compiled

    );
 
    public static string Texto(string? valor, string campo, int min, int max)

    {

        var t = (valor ?? "").Trim();
 
        if (t.Length == 0)

            throw new ArgumentException($"{campo} não pode ser vazio.");
 
        if (t.Length < min)

            throw new ArgumentException($"{campo} deve ter pelo menos {min} caracteres.");
 
        if (t.Length > max)

            throw new ArgumentException($"{campo} deve ter no máximo {max} caracteres.");
 
        return t;

    }
 
    public static string Email(string? valor)

    {

        var t = Texto(valor, "E-mail", 5, 120).ToLowerInvariant();
 
        if (!EmailRegex.IsMatch(t))

            throw new ArgumentException("E-mail em formato inválido.");
 
        return t;

    }
 
    public static string Digitos(string? valor, string campo, int min, int max)

    {

        var t = Texto(valor, campo, min, max);
 
        if (!t.All(char.IsDigit))

            throw new ArgumentException($"{campo} deve conter apenas números.");
 
        return t;

    }

}

 