
using MySqlConnector;

namespace ComunicacaoEscolar.Database;

public class Conexao
{
    private readonly string _connectionString =
        "Server=127.0.0.1;Port=3306;Database=comunicacao_escolar;User ID=root;Password=Senac2026;";

    public MySqlConnection ObterConexao()
    {
        return new MySqlConnection(_connectionString);
    }
}

//correção no ultimo commit eu tinha crido a database, a conexão e o model do usuario