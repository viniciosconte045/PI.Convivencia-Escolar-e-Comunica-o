using System;
using ComunicacaoEscolar.Models;
using ComunicacaoEscolar.Services;


namespace ComunicacaoEscolar.Menus;
public static class MenuFuncionario
{
    public static void Exibir(Usuario usuarioLogado)
    {
        int opcao;

        AvisoService avisoService = new AvisoService();

        do
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("       MENU DO FUNCIONÁRIO");
            Console.WriteLine("================================");
            Console.WriteLine("1 - Enviar aviso");
            Console.WriteLine("2 - Acessar avisos");
            Console.WriteLine("3 - Editar aviso");
            Console.WriteLine("4 - Excluir aviso");
            Console.WriteLine("5 - Ver conversas");
            Console.WriteLine("0 - Sair da conta");
            Console.WriteLine("================================");

            Console.Write("\nEscolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao) || opcao < 0 || opcao > 5)
            {
                Console.WriteLine("\nOpção inválida!");
                Console.WriteLine("Pressione ENTER para voltar ao menu.");
                Console.ReadLine();
                continue;
            }

            switch (opcao)
            {
                case 1:
                {
                    Console.Clear();

                    Console.WriteLine("===== ENVIAR AVISO GERAL =====");

                    Console.Write("\nTítulo: ");
                    string titulo = Console.ReadLine() ?? "";

                    Console.Write("Conteúdo: ");
                    string conteudo = Console.ReadLine() ?? "";

                    if (string.IsNullOrWhiteSpace(titulo) ||
                        string.IsNullOrWhiteSpace(conteudo))
                    {
                        Console.WriteLine("\nPreencha todos os campos!");
                    }
                    else
                    {
                        Console.WriteLine("\n===== CONFIRMAR AVISO =====");
                        Console.WriteLine($"Título: {titulo}");
                        Console.WriteLine($"Conteúdo: {conteudo}");

                        Console.Write("\nDeseja realmente publicar este aviso? (s/n): ");
                        string confirmacao = Console.ReadLine() ?? "";

                        if (confirmacao.ToLower() == "s")
                        {
                            avisoService.CadastrarAviso(
                                usuarioLogado.IdUsuario,
                                titulo,
                                conteudo
                            );

                            Console.WriteLine("\nAviso publicado com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("\nPublicação cancelada.");
                        }
                    }
                    break;
                }

                case 2:
                    
                {
                    Console.Clear();

                    Console.WriteLine("===== AVISOS GERAIS =====\n");

                    avisoService.ListarAvisos();

                    break;
                }

                case 4:
                    {
                        Console.Clear();

                        Console.WriteLine("===== EXCLUIR AVISO =====\n");

                        avisoService.ListarAvisos();

                        Console.Write("\nDigite o ID do aviso: ");

                        if (!int.TryParse(Console.ReadLine(), out int idAviso))
                        {
                            Console.WriteLine("\nID inválido!");
                            break;
                        }

                        Console.Write("\nTem certeza que deseja excluir este aviso? (s/n): ");
                        string confirmacao = Console.ReadLine() ?? "";

                        if (confirmacao.ToLower() == "s")
                        {
                            if (avisoService.ExcluirAviso(idAviso))
                            {
                                Console.WriteLine("\nAviso excluído com sucesso!");
                            }
                            else
                            {
                                Console.WriteLine("\nAviso não encontrado!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nExclusão cancelada.");
                        }

                        break;
                    }
                
                case 3:
                    {
                        Console.Clear();

                        Console.WriteLine("===== EDITAR AVISO =====\n");

                        avisoService.ListarAvisos();

                        Console.Write("\nDigite o ID do aviso: ");

                        if (!int.TryParse(Console.ReadLine(), out int idAviso))
                        {
                            Console.WriteLine("\nID inválido!");
                            break;
                        }

                        Console.Write("\nNovo título: ");
                        string titulo = Console.ReadLine() ?? "";

                        Console.Write("Novo conteúdo: ");
                        string conteudo = Console.ReadLine() ?? "";

                        if (string.IsNullOrWhiteSpace(titulo) ||
                            string.IsNullOrWhiteSpace(conteudo))
                        {
                            Console.WriteLine("\nPreencha todos os campos!");
                            break;
                        }

                        Console.WriteLine("\n===== CONFIRMAR ALTERAÇÕES =====");
                        Console.WriteLine($"Novo título: {titulo}");
                        Console.WriteLine($"Novo conteúdo: {conteudo}");

                        Console.Write("\nDeseja realmente editar este aviso? (s/n): ");
                        string confirmacao = Console.ReadLine() ?? "";

                        if (confirmacao.ToLower() == "s")
                        {
                            if (avisoService.EditarAviso(idAviso, titulo, conteudo))
                            {
                                Console.WriteLine("\nAviso atualizado com sucesso!");
                            }
                            else
                            {
                                Console.WriteLine("\nAviso não encontrado!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nEdição cancelada.");
                        }

                        break;
                    }
                case 5:
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