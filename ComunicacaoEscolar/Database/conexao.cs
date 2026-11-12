
using MySqlConnector;

namespace ComunicacaoEscolar.Database;

public class Conexao
{
    private readonly string _connectionString =
        Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING")
        ?? throw new InvalidOperationException(
            "A variável MYSQL_CONNECTION_STRING não foi configurada."
        );

    public MySqlConnection ObterConexao()
    {
        return new MySqlConnection(_connectionString);
    }
}
