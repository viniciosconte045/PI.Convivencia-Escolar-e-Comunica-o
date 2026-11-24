
using MySqlConnector;
using ComunicacaoEscolar.Database;

namespace ComunicacaoEscolar.Services;

public class ChatService
{
    // Lista os alunos e funcionários disponíveis para conversar.
    public void ListarContatos(int idUsuarioLogado, string tipoUsuarioLogado)
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();

        string sql = @"SELECT id_usuario, nome, tipo, cargo
                       FROM usuario
                       WHERE id_usuario <> @idUsuario
                       AND (
                            (@tipoUsuario = 'aluno' AND tipo = 'funcionario')
                            OR
                            (@tipoUsuario = 'funcionario' AND tipo IN ('aluno', 'funcionario'))
                        )
                       ORDER BY nome"; 

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);

        comandoSql.Parameters.AddWithValue("@idUsuario", idUsuarioLogado);
        comandoSql.Parameters.AddWithValue("@tipoUsuario", tipoUsuarioLogado);

        using MySqlDataReader leitor = comandoSql.ExecuteReader();

        bool encontrou = false;

        while (leitor.Read())
        {
            encontrou = true;

            Console.WriteLine("\n------------------------------");
            Console.WriteLine($"ID: {leitor.GetInt32("id_usuario")}");
            Console.WriteLine($"Nome: {leitor.GetString("nome")}");

            if (leitor.GetString("tipo") == "funcionario")
            {
                string cargo = leitor.IsDBNull(leitor.GetOrdinal("cargo"))
                    ? "Cargo não informado"
                    : leitor.GetString("cargo");

                Console.WriteLine($"Cargo: {cargo}");
            }
            else
            {
                Console.WriteLine("Aluno");
            }
        }
               
        

        if (!encontrou)
        {
            Console.WriteLine("\nNenhum contato encontrado.");
        }
    }

    // Cria uma conversa ou retorna uma que já existe.
    public int ObterOuCriarConversa(int idUsuarioLogado, int idOutroUsuario)
    {
        if (idUsuarioLogado == idOutroUsuario)
        {
            throw new InvalidOperationException(
                "Você não pode iniciar uma conversa consigo mesmo.");
        }

        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();

        string validarSql =  @"SELECT COUNT(*)
                                FROM usuario u1
                                INNER JOIN usuario u2
                                ON u2.id_usuario = @idOutro
                                WHERE u1.id_usuario = @idLogado
                                AND (
                                    (u1.tipo = 'aluno' AND u2.tipo = 'funcionario')
                                    OR
                                    (u1.tipo = 'funcionario' AND u2.tipo IN ('aluno', 'funcionario'))
                                )";

        using MySqlCommand validar = new MySqlCommand(validarSql, conexao);
        validar.Parameters.AddWithValue("@idLogado", idUsuarioLogado);
        validar.Parameters.AddWithValue("@idOutro", idOutroUsuario);

        if (Convert.ToInt32(validar.ExecuteScalar()) != 1)
        {
            throw new InvalidOperationException(
                "Usuário não encontrado ou não disponível para conversas.");
        }

        // Organiza os IDs para evitar conversas duplicadas.
        int idUsuario1 = Math.Min(idUsuarioLogado, idOutroUsuario);
        int idUsuario2 = Math.Max(idUsuarioLogado, idOutroUsuario);

        string consultarSql = @"SELECT id_conversa
                                FROM conversa
                                WHERE (id_usuario1 = @id1 AND id_usuario2 = @id2)
                                   OR (id_usuario1 = @id2 AND id_usuario2 = @id1)
                                LIMIT 1";

        using MySqlCommand consultar = new MySqlCommand(consultarSql, conexao);
        consultar.Parameters.AddWithValue("@id1", idUsuario1);
        consultar.Parameters.AddWithValue("@id2", idUsuario2);

        object? resultado = consultar.ExecuteScalar();

        if (resultado != null && resultado != DBNull.Value)
        {
            return Convert.ToInt32(resultado);
        }

        string inserirSql = @"INSERT INTO conversa (id_usuario1, id_usuario2)
                              VALUES (@id1, @id2)";

        using MySqlCommand inserir = new MySqlCommand(inserirSql, conexao);
        inserir.Parameters.AddWithValue("@id1", idUsuario1);
        inserir.Parameters.AddWithValue("@id2", idUsuario2);

        inserir.ExecuteNonQuery();

        return Convert.ToInt32(inserir.LastInsertedId);
    }

    // Lista todas as conversas do usuário.
    public void ListarConversas(int idUsuarioLogado, string tipoUsuarioLogado)
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();
        if (tipoUsuarioLogado == "admin")
        {
            Console.WriteLine("\nAdministradores não possuem acesso ao chat.");
            return;
        }

        string sql = @"SELECT c.id_conversa,
                              contato.id_usuario AS id_contato,
                              contato.nome,
                              contato.tipo,
                              contato.cargo
                       FROM conversa c
                       INNER JOIN usuario contato
                       ON contato.id_usuario =
                          CASE
                              WHEN c.id_usuario1 = @idUsuario
                              THEN c.id_usuario2
                              ELSE c.id_usuario1
                          END
                        WHERE (c.id_usuario1 = @idUsuario
                          OR c.id_usuario2 = @idUsuario)
                        AND (
                            (@tipoUsuario = 'aluno' AND contato.tipo = 'funcionario')
                            OR
                            (@tipoUsuario = 'funcionario' AND contato.tipo IN ('aluno', 'funcionario'))
                        )
                       ORDER BY c.data_criacao DESC";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);
        comandoSql.Parameters.AddWithValue("@idUsuario", idUsuarioLogado);
        comandoSql.Parameters.AddWithValue("@tipoUsuario", tipoUsuarioLogado);

        using MySqlDataReader leitor = comandoSql.ExecuteReader();

        bool encontrou = false;

        while (leitor.Read())
        {
            encontrou = true;

            Console.WriteLine(
                $"\nID da conversa: {leitor.GetInt32("id_conversa")}");
            Console.WriteLine(
                $"Contato: {leitor.GetString("nome")}");
            Console.WriteLine(
                $"ID do contato: {leitor.GetInt32("id_contato")}");
            if (leitor.GetString("tipo") == "funcionario")
            {
                    string cargo = leitor.IsDBNull(leitor.GetOrdinal("cargo"))
                    ? "Cargo não informado"
                    : leitor.GetString("cargo");

                Console.WriteLine($"Cargo: {cargo}");
            }
            else
            {
                Console.WriteLine("Aluno");
                
            }
        }

        if (!encontrou)
        {
            Console.WriteLine("\nVocê ainda não possui conversas.");
        }
}


    // Mostra o histórico de mensagens.
    public bool ListarMensagens(int idConversa, int idUsuarioLogado)
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();

        string validarSql = @"SELECT COUNT(*)
                              FROM conversa
                              WHERE id_conversa = @idConversa
                              AND (id_usuario1 = @idUsuario
                                   OR id_usuario2 = @idUsuario)";

        using MySqlCommand validar = new MySqlCommand(validarSql, conexao);
        validar.Parameters.AddWithValue("@idConversa", idConversa);
        validar.Parameters.AddWithValue("@idUsuario", idUsuarioLogado);

        if (Convert.ToInt32(validar.ExecuteScalar()) == 0)
        {
            Console.WriteLine("\nConversa não encontrada.");
            return false;
        }

        string sql = @"SELECT mensagem_chat.id_mensagem,
                              usuario.nome,
                              mensagem_chat.conteudo,
                              mensagem_chat.data_envio
                       FROM mensagem_chat
                       INNER JOIN usuario
                       ON mensagem_chat.id_remetente = usuario.id_usuario
                       INNER JOIN conversa
                       ON mensagem_chat.id_conversa = conversa.id_conversa
                       WHERE mensagem_chat.id_conversa = @idConversa
                       AND (conversa.id_usuario1 = @idUsuario
                            OR conversa.id_usuario2 = @idUsuario)
                       ORDER BY mensagem_chat.data_envio ASC";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);
        comandoSql.Parameters.AddWithValue("@idConversa", idConversa);
        comandoSql.Parameters.AddWithValue("@idUsuario", idUsuarioLogado);

        using MySqlDataReader leitor = comandoSql.ExecuteReader();

        Console.WriteLine("\n===== HISTÓRICO DA CONVERSA =====");

        bool encontrou = false;

        while (leitor.Read())
        {
            encontrou = true;
            Console.WriteLine($"\nID da mensagem: {leitor.GetInt32("id_mensagem")}");
            Console.WriteLine($"\n{leitor.GetString("nome")}");
            Console.WriteLine(
                $"{leitor.GetDateTime("data_envio"):dd/MM/yyyy HH:mm}");
            Console.WriteLine(leitor.GetString("conteudo"));
            Console.WriteLine("--------------------------------");
        }

        if (!encontrou)
        {
            Console.WriteLine("\nEssa conversa ainda não possui mensagens.");
        }

        return true;
    }

    // Envia uma mensagem.
    public bool EnviarMensagem(int idConversa, int idRemetente, string conteudo)
    {
        conteudo = conteudo.Trim();

        if (string.IsNullOrWhiteSpace(conteudo))
        {
            return false;
        }

        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();

        string validarSql = @"SELECT COUNT(*)
                              FROM conversa
                              WHERE id_conversa = @idConversa
                              AND (id_usuario1 = @idRemetente
                                   OR id_usuario2 = @idRemetente)";

        using MySqlCommand validar = new MySqlCommand(validarSql, conexao);
        validar.Parameters.AddWithValue("@idConversa", idConversa);
        validar.Parameters.AddWithValue("@idRemetente", idRemetente);

        if (Convert.ToInt32(validar.ExecuteScalar()) == 0)
        {
            return false;
        }

        string sql = @"INSERT INTO mensagem_chat
                       (id_conversa, id_remetente, conteudo)
                       VALUES (@idConversa, @idRemetente, @conteudo)";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);
        comandoSql.Parameters.AddWithValue("@idConversa", idConversa);
        comandoSql.Parameters.AddWithValue("@idRemetente", idRemetente);
        comandoSql.Parameters.AddWithValue("@conteudo", conteudo);

        return comandoSql.ExecuteNonQuery() > 0;
    }

    // Edita uma mensagem enviada pelo próprio usuário.
    public bool EditarMensagem(int idMensagem, int idUsuario, string novoConteudo)
    {
        novoConteudo = novoConteudo.Trim();

        if (string.IsNullOrWhiteSpace(novoConteudo))
        {
            return false;
        }

        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();

        string sql = @"UPDATE mensagem_chat
                    SET conteudo = @conteudo
                    WHERE id_mensagem = @idMensagem
                    AND id_remetente = @idUsuario";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);

        comandoSql.Parameters.AddWithValue("@conteudo", novoConteudo);
        comandoSql.Parameters.AddWithValue("@idMensagem", idMensagem);
        comandoSql.Parameters.AddWithValue("@idUsuario", idUsuario);

        return comandoSql.ExecuteNonQuery() > 0;
    }

    // Exclui uma mensagem enviada pelo próprio usuário.
    public bool ExcluirMensagem(int idMensagem, int idUsuario)
    {
        using MySqlConnection conexao = new Conexao().ObterConexao();
        conexao.Open();

        string sql = @"DELETE FROM mensagem_chat
                    WHERE id_mensagem = @idMensagem
                    AND id_remetente = @idUsuario";

        using MySqlCommand comandoSql = new MySqlCommand(sql, conexao);

        comandoSql.Parameters.AddWithValue("@idMensagem", idMensagem);
        comandoSql.Parameters.AddWithValue("@idUsuario", idUsuario);

        return comandoSql.ExecuteNonQuery() > 0;
    }
}
