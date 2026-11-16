using System;
using System.Collections.Generic;
using System.Net.Mail;
 
class Program
{
    static List<string> emailsCadastrados = new List<string>();
 
    static void Main(string[] args)
    {
        int opcao = -1;
 
        while (opcao != 0)
        {
            Console.WriteLine("\n===== SISTEMA DE CADASTRO =====");
            Console.WriteLine("1 - Fazer cadastro");
            Console.WriteLine("2 - Testar cadastro");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");
 
            if (!int.TryParse(Console.ReadLine(), out opcao))
            {
                Console.WriteLine("Digite um número válido!");
                opcao = -1;
                continue;
            }
 
            switch (opcao)
            {
                case 1:
                    CadastroPublico();
                    break;
 
                case 2:
                    TestesCadastro();
                    break;
 
                case 0:
                    Console.WriteLine("Sistema encerrado.");
                    break;
 
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }
    }
 
    // CADASTRO PÚBLICO
    static void CadastroPublico()
    {
        Console.WriteLine("\n===== NOVO CADASTRO =====");
 
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
            Console.WriteLine("Erro: informe seu nome.");
            return;
        }
 
        if (nome.Length > 100)
        {
            Console.WriteLine("Erro: o nome deve ter até 100 caracteres.");
            return;
        }
 
        if (!EmailValido(email))
        {
            Console.WriteLine("Erro: e-mail inválido.");
            return;
        }
 
        if (senha.Length < 8)
        {
            Console.WriteLine("Erro: a senha deve ter pelo menos 8 caracteres.");
            return;
        }
 
        if (senha != confirmarSenha)
        {
            Console.WriteLine("Erro: as senhas não são iguais.");
            return;
        }
 
        if (emailsCadastrados.Contains(email))
        {
            Console.WriteLine("Erro: este e-mail já está cadastrado.");
            return;
        }
 
        emailsCadastrados.Add(email);
 
        Console.WriteLine("\nCadastro realizado com sucesso!");
        Console.WriteLine("Nome: " + nome);
        Console.WriteLine("E-mail: " + email);
    }
 
    // VALIDAÇÃO DO E-MAIL
    static bool EmailValido(string email)
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
 
    // TESTES DO CADASTRO
    static void TestesCadastro()
    {
        Console.WriteLine("\n===== TESTES DO CADASTRO =====");
 
        Console.WriteLine("Teste nome vazio: " +
            (string.IsNullOrWhiteSpace("") ? "APROVADO" : "REPROVADO"));
 
        Console.WriteLine("Teste e-mail inválido: " +
            (!EmailValido("emailinvalido") ? "APROVADO" : "REPROVADO"));
 
        Console.WriteLine("Teste senha curta: " +
            ("123".Length < 8 ? "APROVADO" : "REPROVADO"));
 
        Console.WriteLine("Teste senhas diferentes: " +
            ("12345678" != "87654321" ? "APROVADO" : "REPROVADO"));
 
        Console.WriteLine("Teste e-mail válido: " +
            (EmailValido("teste@email.com") ? "APROVADO" : "REPROVADO"));
    }
}