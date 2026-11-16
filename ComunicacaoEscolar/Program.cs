using ComunicacaoEscolar.Services;
using ComunicacaoEscolar.Models;

LoginService loginService = new LoginService();

Console.WriteLine("================================");
Console.WriteLine("       SISTEMA ESCOLAR");
Console.WriteLine("================================");

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
        return;
    }

    Console.WriteLine("\nLogin realizado com sucesso!");
    Console.WriteLine($"Nome: {usuario.Nome}");
    Console.WriteLine($"Tipo: {usuario.Tipo}");

    if (usuario.Tipo == "admin")
    {
        MenuAdministrador();
    }
    else
    {
        Console.WriteLine("\nMenu ainda não implementado para esse tipo de usuário.");
    }
}
catch (Exception erro)
{
    Console.WriteLine($"\nErro ao realizar login: {erro.Message}");
}

static void MenuAdministrador()
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
                Console.WriteLine("\n===== LISTA DE USUÁRIOS =====");

                List<Usuario> usuarios = usuarioService.ListarUsuarios();

                if (usuarios.Count == 0)
                {
                    Console.WriteLine("\nNenhum usuário cadastrado.");
                }
                else
                {
                    foreach (Usuario u in usuarios)
                    {
                        Console.WriteLine("\n------------------------------");
                        Console.WriteLine($"ID: {u.IdUsuario}");
                        Console.WriteLine($"Nome: {u.Nome}");
                        Console.WriteLine($"E-mail: {u.Email}");
                        Console.WriteLine($"Tipo: {u.Tipo}");
                    }

                    Console.WriteLine("------------------------------");
                }

                break;

            case 3:
                Console.WriteLine("\nConsulta de usuário");
                break;

            case 4:
                Console.WriteLine("\nEdição de usuário");
                break;

            case 5:
                Console.WriteLine("\nExclusão de usuário");
                break;

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


