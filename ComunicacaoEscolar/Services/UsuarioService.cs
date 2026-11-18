
using MySqlConnector;
using ComunicacaoEscolar.Database;
using ComunicacaoEscolar.Models;

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

     public List<Usuario> ListarUsuarios()
    {
        List<Usuario> usuarios = new List<Usuario>();

        using MySqlConnection conexao = new Conexao().ObterConexao();

        conexao.Open();

        string sql = "SELECT * FROM usuario";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);

        using MySqlDataReader leitor = comandoSql.ExecuteReader();

        while (leitor.Read())
        {
            Usuario usuario = new Usuario(
                leitor.GetInt32("id_usuario"),
                leitor.GetString("nome"),
                leitor.GetString("email"),
                leitor.GetString("tipo"),
                leitor.GetString("senha")
            );

            usuarios.Add(usuario);
        }

        return usuarios;
    }
    public void EditarUsuario(int id, string nome, string email, string senha, string tipo)
    {
    using MySqlConnection conexao = new Conexao().ObterConexao();

    conexao.Open();

    string sql = @"UPDATE usuario
                   SET nome = @nome,
                       email = @email,
                       senha = @senha,
                       tipo = @tipo
                   WHERE id_usuario = @id";

    using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);

    comandoSql.Parameters.AddWithValue("@id", id);
    comandoSql.Parameters.AddWithValue("@nome", nome);
    comandoSql.Parameters.AddWithValue("@email", email);
    comandoSql.Parameters.AddWithValue("@senha", senha);
    comandoSql.Parameters.AddWithValue("@tipo", tipo);

    comandoSql.ExecuteNonQuery();
}

    public Usuario? ConsultarUsuarioPorId(int id)
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();

        conexao.Open();

        string sql = @"SELECT id_usuario, nome, email, tipo, senha
                    FROM usuario
                    WHERE id_usuario = @id";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);

        comandoSql.Parameters.AddWithValue("@id", id);

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

    public void ExcluirUsuario(int id)
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();

        string sql = "DELETE FROM usuario WHERE id_usuario = @id";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);
        comandoSql.Parameters.AddWithValue("@id", id);

        comandoSql.ExecuteNonQuery();
    }

    public bool EmailExiste(string email)
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();

        string sql = "SELECT COUNT(*) FROM usuario WHERE email = @email";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);
        comandoSql.Parameters.AddWithValue("@email", email);

        int quantidade = Convert.ToInt32(comandoSql.ExecuteScalar());

        return quantidade > 0;
    }

    public void CadastrarAluno(string nome, string email, string senha)
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();

        string sql = @"INSERT INTO usuario (nome, email, senha, tipo)
                    VALUES (@nome, @email, @senha, 'aluno')";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);

        comandoSql.Parameters.AddWithValue("@nome", nome);
        comandoSql.Parameters.AddWithValue("@email", email);
        comandoSql.Parameters.AddWithValue("@senha", senha);

        comandoSql.ExecuteNonQuery();
    }
}
