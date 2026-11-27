using MySqlConnector;

namespace ComunicacaoEscolar.Database;

public static class CriarDatabase
{
    public static void CriarBanco()
    {
        string conexaoServidor = "Server=localhost;Port=3306;User ID=root;Password=Senac2026;";

        using var conexao = new MySqlConnection(conexaoServidor);
        conexao.Open();

        string sqlBanco = "CREATE DATABASE IF NOT EXISTS comunicacao_escolar;";
        using var comando = new MySqlCommand(sqlBanco, conexao);
        comando.ExecuteNonQuery();

        conexao.ChangeDatabase("comunicacao_escolar");

        string[] comandos =
        {
            @"CREATE TABLE IF NOT EXISTS usuario (
                id_usuario INT AUTO_INCREMENT PRIMARY KEY,
                nome VARCHAR(100) NOT NULL,
                email VARCHAR(100) NOT NULL UNIQUE,
                senha VARCHAR(255) NOT NULL,
                tipo ENUM('aluno', 'funcionario', 'admin') NOT NULL,
                cargo VARCHAR(100) NULL
            );",

            @"CREATE TABLE IF NOT EXISTS aviso (
                id_aviso INT AUTO_INCREMENT PRIMARY KEY,
                id_autor INT NOT NULL,
                titulo VARCHAR(100) NOT NULL,
                conteudo TEXT NOT NULL,
                data_publicacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (id_autor)
                    REFERENCES usuario(id_usuario)
                    ON DELETE CASCADE
            );",

            @"CREATE TABLE IF NOT EXISTS conversa (
                id_conversa INT AUTO_INCREMENT PRIMARY KEY,
                id_usuario1 INT NOT NULL,
                id_usuario2 INT NOT NULL,
                data_criacao DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (id_usuario1) REFERENCES usuario(id_usuario),
                FOREIGN KEY (id_usuario2) REFERENCES usuario(id_usuario),
                UNIQUE (id_usuario1, id_usuario2),
                CHECK (id_usuario1 <> id_usuario2)
            );",

            @"CREATE TABLE IF NOT EXISTS mensagem_chat (
                id_mensagem INT AUTO_INCREMENT PRIMARY KEY,
                id_conversa INT NOT NULL,
                id_remetente INT NOT NULL,
                conteudo TEXT NOT NULL,
                data_envio DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (id_conversa)
                    REFERENCES conversa(id_conversa)
                    ON DELETE CASCADE,
                FOREIGN KEY (id_remetente)
                    REFERENCES usuario(id_usuario)
            );",

            @"INSERT IGNORE INTO usuario 
                (nome, email, senha, tipo, cargo)
                VALUES ('Administrador', 'admin@escola.com', 
                '123456', 'admin', NULL);",
            @"INSERT IGNORE INTO usuario
                (nome, email, senha, tipo, cargo)
                VALUES ('Funcionario', 'funco@escola.com',
                '12345678', 'funcionario', 'Professor');",

            @"INSERT IGNORE INTO usuario
                (nome, email, senha, tipo, cargo)
                VALUES ('Aluno', 'aluno@gmail.com',
                '12345678', 'aluno', NULL);"
        };

        foreach (string sql in comandos)
        {
            using var cmd = new MySqlCommand(sql, conexao);
            cmd.ExecuteNonQuery();
        }

        Console.WriteLine("Banco de dados criado com sucesso!");
    }
}