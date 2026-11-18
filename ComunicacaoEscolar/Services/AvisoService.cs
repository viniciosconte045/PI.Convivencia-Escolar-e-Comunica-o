using MySqlConnector;
using ComunicacaoEscolar.Database;

namespace ComunicacaoEscolar.Services;

public class AvisoService
{
    public void CadastrarAviso(int idAutor, string titulo, string conteudo)
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();

        string sql = @"INSERT INTO aviso (id_autor, titulo, conteudo)
                       VALUES (@idAutor, @titulo, @conteudo)";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);

        comandoSql.Parameters.AddWithValue("@idAutor", idAutor);
        comandoSql.Parameters.AddWithValue("@titulo", titulo);
        comandoSql.Parameters.AddWithValue("@conteudo", conteudo);

        comandoSql.ExecuteNonQuery();
    }

    public void ListarAvisos()
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();

        string sql = @"SELECT aviso.id_aviso, aviso.titulo, aviso.conteudo,
                              aviso.data_publicacao, usuario.nome
                       FROM aviso
                       INNER JOIN usuario
                       ON aviso.id_autor = usuario.id_usuario
                       ORDER BY aviso.data_publicacao DESC";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);
        using MySqlDataReader leitor = comandoSql.ExecuteReader();

        while (leitor.Read())
        {
            Console.WriteLine("\n================================");
            Console.WriteLine($"Título: {leitor.GetString("titulo")}");
            Console.WriteLine($"Autor: {leitor.GetString("nome")}");
            Console.WriteLine($"Data: {leitor.GetDateTime("data_publicacao")}");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(leitor.GetString("conteudo"));
            Console.WriteLine("================================");
        }
    }
}