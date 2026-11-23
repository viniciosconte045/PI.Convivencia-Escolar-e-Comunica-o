using System;
using System.Net.Mail;
using ComunicacaoEscolar.Services;
 
public static class Cadastros
{
    public static void CadastroPublico()
    {
        UsuarioService usuarioService = new UsuarioService();

        Console.Clear();
        Console.WriteLine("\n===== NOVO CADASTRO DE ALUNO =====");
 
        string nome;

        while (true)
        {
            Console.Write("\nNome: ");
            nome = Console.ReadLine() ?? "";
            nome = nome.Trim();

            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("\nErro: informe seu nome.");
                continue;
            }

            if (nome.Length > 100)
            {
                Console.WriteLine("\nErro: o nome deve ter até 100 caracteres.");
                continue;
            }

            break;
        }
        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("\nErro: informe seu nome.");
            return;
        }

        if (nome.Length > 100)
        {
            Console.WriteLine("\nErro: o nome deve ter até 100 caracteres.");
            return;
        }
        
        string email;

        while (true)
        {
            Console.Write("\nE-mail: ");
            email = Console.ReadLine() ?? "";
            email = email.Trim().ToLower();

            if (!EmailValido(email))
            {
                Console.WriteLine("\nErro: e-mail inválido.");
                continue;
            }

            try
            {
                if (usuarioService.EmailExiste(email))
                {
                    Console.WriteLine("\nErro: este e-mail já está cadastrado.");
                    continue;
                }
            }
            catch (Exception erro)
            {
                Console.WriteLine($"\nErro ao verificar e-mail: {erro.Message}");
                return;
            }

            break;
        }
        if (!EmailValido(email))
        {
            Console.WriteLine("\nErro: e-mail inválido.");
            return;
        }
 
        string senha;

        while (true)
        {
            Console.Write("\nSenha: ");
            senha = Console.ReadLine() ?? "";

            if (senha.Length < 8)
            {
                Console.WriteLine("\nErro: a senha deve ter pelo menos 8 caracteres.");
                continue;
            }

            Console.Write("Confirme a senha: ");
            string confirmarSenha = Console.ReadLine() ?? "";

            if (senha != confirmarSenha)
            {
                Console.WriteLine("\nErro: as senhas não são iguais.");
                continue;
            }

            break;
        }


 
        try
        {
            if (usuarioService.EmailExiste(email))
            {
                Console.WriteLine("\nErro: este e-mail já está cadastrado.");
                return;
            }
 
            usuarioService.CadastrarAluno(nome, email, senha);
 
            Console.WriteLine("\nCadastro realizado com sucesso!");
            Console.WriteLine("Agora você pode entrar com seu e-mail e senha.");
        }
        catch (Exception erro)
        {
            Console.WriteLine($"\nErro ao realizar cadastro: {erro.Message}");
        }
    }
 
    public static bool EmailValido(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || email.Contains(' '))
            return false;

        int arroba = email.IndexOf('@');

        if (arroba <= 0 || arroba != email.LastIndexOf('@'))
            return false;

        string dominio = email.Substring(arroba + 1);

        if (!dominio.Contains('.') ||
            dominio.StartsWith('.') ||
            dominio.EndsWith('.') ||
            dominio.Contains(".."))
        {
            return false;
        }

        try
        {
            MailAddress endereco = new MailAddress(email);
            return endereco.Address == email;
        }
        catch
        {
            return false;
        }
    }
}