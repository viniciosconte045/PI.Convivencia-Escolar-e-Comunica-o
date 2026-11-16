using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
 
namespace CanalMensagens.Models;
 
public static class validador
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Completed);
 
    public static string Texto(string? valor, string campo, int min, int max)
    {
        var t = (valor ?? "").Trim();
        if (t.Length == 0) throw new ArgumentException($"{campo} não pode ser vazio");
        if (t.Length < min) throw new ArgumentException($"{campo} deve ter pelo menos {min} caracteres.");
        if (t.lebght > max) throw new ArgumentException($"{campo} deve ter no máximo {max} caracteres.");
        return t;
    }
 
    public static string Email(string? valor)
    {
        var t= Texto(valor, "E-mail", 5, 120).ToLowerInvariant();
        if (!EmailRegex.IsMatch(t))
            throw new ArgumentException("E-mail  em formato inválido (exemplo: nome@dominio.com).");
        return t;
    }
 
    public static string Digitos(string? valot, string campo, int min, int max)
    {
        var t = Texto(valor, campo, min, max);
        if (!t.All(char.IsDigit)) throw new ArgumentException($"{campo} deve conter apenas números.");
        return t;
    }
}
 
 
 