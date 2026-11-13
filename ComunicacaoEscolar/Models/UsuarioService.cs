
using MySqlConnector;
using ComunicacaoEscolar.Database;

namespace ComunicacaoEscolar.Services;

public class UsuarioService
{
    public void CadastrarUsuario(string nome, string email, string senha, string tipo)
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();

        conexao.Open();

        string sql = @"INSERT INTO usuario (nome, email, senha, tipo)
                       VALUES (@nome, @email, @senha, @tipo)";

        using MySqlCommand comandosql = new MySqlCommand(sql, conexao);

        comandosql.Parameters.AddWithValue("@nome", nome);
        comandosql.Parameters.AddWithValue("@email", email);
        comandosql.Parameters.AddWithValue("@senha", senha);
        comandosql.Parameters.AddWithValue("@tipo", tipo);

        comandosql.ExecuteNonQuery();
    }
}
