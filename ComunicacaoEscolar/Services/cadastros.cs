using System;
using System.Net.Mail;
using ComunicacaoEscolar.Services;
 
public static class Cadastros
{
    public static void CadastroPublico()
    {
        UsuarioService usuarioService = new UsuarioService();
 
        Console.WriteLine("\n===== NOVO CADASTRO DE ALUNO =====");
 
        Console.Write("Nome: ");
        string nome = Console.ReadLine() ?? "";
 
        Console.Write("E-mail: ");
        string email = Console.ReadLine() ?? "";
        email = email.Trim().ToLower();
 
        Console.Write("Senha: ");
        string senha = Console.ReadLine() ?? "";
 
        Console.Write("Confirme a senha: ");
        string confirmarSenha = Console.ReadLine() ?? "";
 
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
 
        if (!EmailValido(email))
        {
            Console.WriteLine("\nErro: e-mail inválido.");
            return;
        }
 
        if (senha.Length < 8)
        {
            Console.WriteLine("\nErro: a senha deve ter pelo menos 8 caracteres.");
            return;
        }
 
        if (senha != confirmarSenha)
        {
            Console.WriteLine("\nErro: as senhas não são iguais.");
            return;
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
        try
        {
            MailAddress endereco = new MailAddress(email);
            return endereco.Address == email.Trim();
        }
        catch
        {
            return false;
        }
    }
}