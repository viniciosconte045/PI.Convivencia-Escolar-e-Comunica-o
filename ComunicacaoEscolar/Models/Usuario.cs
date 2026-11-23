
namespace ComunicacaoEscolar.Models;

public class Usuario
{
    public int IdUsuario { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Tipo { get; private set; }

    public string Senha { get; private set; }

    public string? Cargo { get; private set; }

    public Usuario(int idUsuario, string nome, string email, string tipo, string senha, string? cargo = null)
    {
        IdUsuario = idUsuario;
        Nome = nome;
        Email = email;
        Tipo = tipo;
        Senha = senha;
        Cargo = cargo;
    }
}
