
using System;
using ComunicacaoEscolar.Models;
using ComunicacaoEscolar.Services;

namespace ComunicacaoEscolar.Menus;

public static class MenuAluno
{
    public static void Exibir(Usuario usuarioLogado)
    {
        int opcao;
        AvisoService avisoService = new AvisoService();

        do
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("          MENU DO ALUNO");
            Console.WriteLine("================================");

            Console.WriteLine("\n1 - Visualizar avisos");
            Console.WriteLine("2 - Acessae conversas");
            Console.WriteLine("0 - Sair da conta");

            Console.Write("\nEscolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao) ||
                (opcao != 0 && opcao != 1 && opcao != 2))
            {
                Console.WriteLine("\nOpção inválida.");
                Console.ReadLine();
                continue;
            }

            switch (opcao)
            {
                case 1:
                    Console.Clear();

                    Console.WriteLine("===== AVISOS GERAIS =====\n");
                    avisoService.ListarAvisos();
                    break;
                case 2:
                    MenuConversas.Exibir(usuarioLogado);
                    break;
                case 0:
                    Console.WriteLine("\nSaindo da conta...");
                    break;
            }

            if (opcao != 0)
            {
                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
            }

        } while (opcao != 0);
    }
}
