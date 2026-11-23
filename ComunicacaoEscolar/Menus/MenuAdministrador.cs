using System;
using System.Collections.Generic;
using System.Linq;
using ComunicacaoEscolar.Models;
using ComunicacaoEscolar.Services;

namespace ComunicacaoEscolar.Menus;

public static class MenuAdministrador
{       public static void Exibir()
    {
    
    int opcao;
    UsuarioService usuarioService = new UsuarioService();

        do
        {
            Console.Clear();

            Console.WriteLine("\n================================");
            Console.WriteLine("     MENU DO ADMINISTRADOR");
            Console.WriteLine("================================");
            Console.WriteLine("1 - Cadastrar funcionário");
            Console.WriteLine("2 - Listar usuários");
            Console.WriteLine("3 - Consultar usuário por ID");
            Console.WriteLine("4 - Editar usuário");
            Console.WriteLine("5 - Excluir usuário");
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
                    Console.WriteLine("\n===== CADASTRO DE FUNCIONÁRIO =====");

                    Console.Write("Nome: ");
                    string nome = Console.ReadLine() ?? "";

                    Console.Write("E-mail: ");
                    string email = Console.ReadLine() ?? "";

                    Console.Write("Senha: ");
                    string senha = Console.ReadLine() ?? "";

                    if (nome == "" || email == "" || senha == "")
                    {
                        Console.WriteLine("\nPreencha todos os campos!");
                        break;
                    }

                    try
                    {
                        
                        usuarioService.CadastrarUsuario(nome, email, senha, "funcionario");

                        Console.WriteLine("\nFuncionário cadastrado com sucesso!");
                    }
                    catch (Exception erro)
                    {
                        Console.WriteLine($"\nErro ao cadastrar funcionário: {erro.Message}");
                    }

                    break;
                }

                case 2:
                {
                    Console.WriteLine("\n===== LISTAR USUÁRIOS =====");
                    Console.WriteLine("1 - Ordenar por ID");
                    Console.WriteLine("2 - Ordenar por nome");

                    Console.Write("\nEscolha uma opção: ");

                    if (!int.TryParse(Console.ReadLine(), out int ordem) || ordem < 1 || ordem > 2)
                    {
                        Console.WriteLine("\nOpção inválida!");
                        break;
                    }

                    List<Usuario> usuarios = usuarioService.ListarUsuarios();

                    if (usuarios.Count == 0)
                    {
                        Console.WriteLine("\nNenhum usuário cadastrado.");
                        break;
                    }

                    if (ordem == 1)
                    {
                        usuarios = usuarios.OrderBy(u => u.IdUsuario).ToList();
                    }
                    else
                    {
                        usuarios = usuarios.OrderBy(u => u.Nome).ToList();
                    }

                    foreach (Usuario u in usuarios)
                    {
                        Console.WriteLine("\n------------------------------");
                        Console.WriteLine($"ID: {u.IdUsuario}");
                        Console.WriteLine($"Nome: {u.Nome}");
                        Console.WriteLine($"E-mail: {u.Email}");
                        Console.WriteLine($"Tipo: {u.Tipo}");
                    }

                    Console.WriteLine("------------------------------");
                    break;
                }


                case 3:
                {
                    Console.WriteLine("\n===== CONSULTAR USUÁRIO POR ID =====");

                    Console.Write("Digite o ID do usuário: ");

                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("\nID inválido!");
                        break;
                    }

                    Usuario? usuario = usuarioService.ConsultarUsuarioPorId(id);

                    if (usuario == null)
                    {
                        Console.WriteLine("\nUsuário não encontrado!");
                    }
                    else
                    {
                        Console.WriteLine("\n===== USUÁRIO ENCONTRADO =====");
                        Console.WriteLine($"ID: {usuario.IdUsuario}");
                        Console.WriteLine($"Nome: {usuario.Nome}");
                        Console.WriteLine($"E-mail: {usuario.Email}");
                        Console.WriteLine($"Tipo: {usuario.Tipo}");
                    }

                    break;
    }

                case 4:
                {
                    Console.WriteLine("\n===== EDITAR USUÁRIO =====");

                    Console.Write("Digite o ID do usuário: ");

                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("\nID inválido!");
                        break;
                    }

                    Usuario? usuario = usuarioService.ConsultarUsuarioPorId(id);

                    if (usuario == null)
                    {
                        Console.WriteLine("\nUsuário não encontrado!");
                        break;
                    }

                    Console.WriteLine("\n===== DADOS ATUAIS =====");
                    Console.WriteLine($"Nome: {usuario.Nome}");
                    Console.WriteLine($"E-mail: {usuario.Email}");
                    Console.WriteLine($"Tipo: {usuario.Tipo}");

                    Console.WriteLine("\n===== NOVOS DADOS =====");

                    Console.Write("Novo nome: ");
                    string nome = Console.ReadLine() ?? "";

                    Console.Write("Novo e-mail: ");
                    string email = Console.ReadLine() ?? "";

                    Console.Write("Nova senha: ");
                    string senha = Console.ReadLine() ?? "";

                    Console.Write("Novo tipo (aluno, funcionario ou admin): ");
                    string tipo = Console.ReadLine() ?? "";

                    if (nome == "" || email == "" || senha == "" || tipo == "")
                    {
                        Console.WriteLine("\nPreencha todos os campos!");
                        break;
                    }

                    usuarioService.EditarUsuario(id, nome, email, senha, tipo);

                    Console.WriteLine("\nUsuário atualizado com sucesso!");

                    break;
                }

                case 5:
                {
                    Console.WriteLine("\n===== EXCLUIR USUÁRIO =====");

                    Console.Write("Digite o ID do usuário: ");

                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("\nID inválido!");
                        break;
                    }

                    Usuario? usuario = usuarioService.ConsultarUsuarioPorId(id);

                    if (usuario == null)
                    {
                        Console.WriteLine("\nUsuário não encontrado!");
                        break;
                    }

                    Console.WriteLine($"\nNome: {usuario.Nome}");
                    Console.WriteLine($"E-mail: {usuario.Email}");
                    Console.WriteLine($"Tipo: {usuario.Tipo}");

                    Console.Write("\nTem certeza que deseja excluir? (s/n): ");
                    string confirmacao = Console.ReadLine() ?? "";

                    if (confirmacao.ToLower() == "s")
                    {
                        usuarioService.ExcluirUsuario(id);
                        Console.WriteLine("\nUsuário excluído com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("\nExclusão cancelada.");
                    }

                    break;
                }
                
                case 0:
                    Console.WriteLine("\nSaindo da conta...");
                    break;

                default:
                    Console.WriteLine("\nOpção inexistente.");
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