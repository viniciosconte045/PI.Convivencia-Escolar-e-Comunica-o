using MySqlConnector;
using ComunicacaoEscolar.Database;

namespace ComunicacaoEscolar.Models;

public class LoginService
{
    public Usuario? Autenticar(string email, string senha)
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();

        conexao.Open();

        string sql = @"SELECT id_usuario, nome, email, tipo, senha
                       FROM usuario
                       WHERE email = @email AND senha = @senha";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);

        comandoSql.Parameters.AddWithValue("@email", email);
        comandoSql.Parameters.AddWithValue("@senha", senha);

        using MySqlDataReader leitor = comandoSql.ExecuteReader();

        if (leitor.Read())
        {
            return new Usuario(
                leitor.GetInt32("id_usuario"),
                leitor.GetString("nome"),
                leitor.GetString("email"),
                leitor.GetString("tipo"),
                leitor.GetString("senha")
            );
        }

        return null;
    }
}
