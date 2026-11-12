
namespace ComunicacaoEscolar.Models;

public class Usuario
{
    public int IdUsuario { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Tipo { get; private set; }

    public Usuario(int idUsuario, string nome, string email, string tipo)
    {
        IdUsuario = idUsuario;
        Nome = nome;
        Email = email;
        Tipo = tipo;
    }
}
