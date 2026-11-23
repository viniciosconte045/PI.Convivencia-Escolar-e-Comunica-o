using System;
using ComunicacaoEscolar.Services;
using ComunicacaoEscolar.Models;
using ComunicacaoEscolar.Menus;

LoginService loginService = new LoginService();




while (true)
{
    Console.Clear();

    Console.WriteLine("================================");
    Console.WriteLine("         SISTEMA ESCOLAR");
    Console.WriteLine("================================");

    Console.WriteLine("\n1 - Fazer login");
    Console.WriteLine("2 - Criar conta");
    Console.WriteLine("0 - Sair");

    Console.Write("\nEscolha uma opção: ");

    if (!int.TryParse(Console.ReadLine(), out int opcao))
    {
        Console.WriteLine("\nOpção inválida.");
        Console.ReadLine();
        continue;
    }

    if (opcao == 0)
    {
        break;
    }

    if (opcao == 2)
    {
        Cadastros.CadastroPublico();

        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
        continue;
    }

    if (opcao != 1)
    {
        Console.WriteLine("\nOpção inválida.");
        Console.ReadLine();
        continue;
    }

    Console.Write("\nE-mail: ");
    string email = Console.ReadLine() ?? "";

    Console.Write("Senha: ");
    string senha = Console.ReadLine() ?? "";

    try
    {
        Usuario? usuario = loginService.Autenticar(email, senha);

        if (usuario == null)
        {
            Console.WriteLine("\nE-mail ou senha incorretos.");
            Console.ReadLine();
            continue;
        }

        Console.WriteLine("\nLogin realizado com sucesso!");
        Console.WriteLine($"Nome: {usuario.Nome}");
        Console.WriteLine($"Tipo: {usuario.Tipo}");

        if (usuario.Tipo == "admin")
        {
            MenuAdministrador.Exibir();
        }
        else if (usuario.Tipo == "funcionario")
        {
            MenuFuncionario.Exibir(usuario);
        }
        else if (usuario.Tipo == "aluno")
        {
            MenuAluno.Exibir(usuario);

        }
    }
    catch (Exception erro)
    {
        Console.WriteLine($"\nErro ao realizar login: {erro.Message}");
        Console.ReadLine();
    }
}



