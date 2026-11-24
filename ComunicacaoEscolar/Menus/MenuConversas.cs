
using System;
using ComunicacaoEscolar.Models;
using ComunicacaoEscolar.Services;

namespace ComunicacaoEscolar.Menus;

public static class MenuConversas
{
    public static void Exibir(Usuario usuarioLogado)
    {
        int opcao;
        ChatService chatService = new ChatService();

        do
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("       MENU DE CONVERSAS");
            Console.WriteLine("================================");

            Console.WriteLine("\n1 - Iniciar nova conversa");
            Console.WriteLine("2 - Visualizar conversas");
            Console.WriteLine("0 - Voltar");

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

                    Console.WriteLine("===== CONTATOS DISPONÍVEIS =====\n");
                    chatService.ListarContatos(usuarioLogado.IdUsuario, usuarioLogado.Tipo);

                    Console.Write("\nDigite o ID do contato: ");

                    if (!int.TryParse(Console.ReadLine(), out int idContato))
                    {
                        Console.WriteLine("\nID inválido.");
                        Console.ReadLine();
                        break;
                    }

                    try
                    {
                        int idConversa = chatService.ObterOuCriarConversa(
                            usuarioLogado.IdUsuario, idContato);

                        AbrirConversa(idConversa, usuarioLogado);
                    }
                    catch (Exception erro)
                    {
                        Console.WriteLine($"\nErro: {erro.Message}");
                        Console.ReadLine();
                    }
                    break;

                case 2:
                    Console.Clear();

                    Console.WriteLine("===== SUAS CONVERSAS =====\n");
                    chatService.ListarConversas(usuarioLogado.IdUsuario, usuarioLogado.Tipo);

                    Console.Write("\nDigite o ID da conversa: ");

                    if (!int.TryParse(Console.ReadLine(), out int idSelecionado))
                    {
                        Console.WriteLine("\nID inválido.");
                        Console.ReadLine();
                        break;
                    }

                    AbrirConversa(idSelecionado, usuarioLogado);
                    break;
            }

        } while (opcao != 0);
    }

    private static void AbrirConversa(int idConversa, Usuario usuarioLogado)
    {
        ChatService chatService = new ChatService();

        while (true)
        {
            Console.Clear();

            bool conversaValida = chatService.ListarMensagens(
                idConversa, usuarioLogado.IdUsuario);

            if (!conversaValida)
            {
                Console.ReadLine();
                break;
            }

            Console.WriteLine("\n1 - Enviar mensagem");
            Console.WriteLine("2 - Editar mensagem");
            Console.WriteLine("3 - Excluir mensagem");
            Console.WriteLine("0 - Voltar");


            Console.Write("\nEscolha uma opção: ");
            string opcao = Console.ReadLine() ?? "";

            if (opcao == "0")
            {
                break;
            }

            if (opcao == "1")
            {
                Console.Write("\nDigite sua mensagem: ");
                string mensagem = Console.ReadLine() ?? "";

                bool enviada = chatService.EnviarMensagem(
                    idConversa, usuarioLogado.IdUsuario, mensagem);

                if (enviada)
                {
                    Console.WriteLine("\nMensagem enviada!");
                }
                else
                {
                    Console.WriteLine("\nNão foi possível enviar a mensagem.");
                }

                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("\nOpção inválida.");
                Console.ReadLine();
            }
        }
    }
}
