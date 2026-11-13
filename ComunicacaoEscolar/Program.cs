using ComunicacaoEscolar.Services;
using ComunicacaoEscolar.Models;

UsuarioService usuarioService = new UsuarioService();

// usuarioService.CadastrarUsuario(
//     "João Silva",
//     "joao@email.com",
//     "123456",
//     "admin"
// );

// Console.WriteLine("Usuário cadastrado com sucesso!");

// List<Usuario> usuarios = usuarioService.ListarUsuarios();

// foreach (Usuario usuario in usuarios)
// {
//     Console.WriteLine("----------------------");
//     Console.WriteLine($"ID: {usuario.IdUsuario}");
//     Console.WriteLine($"Nome: {usuario.Nome}");
//     Console.WriteLine($"Email: {usuario.Email}");
//     Console.WriteLine($"Tipo: {usuario.Tipo}");
// }
// usuarioService.EditarUsuario(
//     1,
//     "João Silva Atualizado",
//     "joao.novo@email.com",
//     "123456",
//     "admin"
// );

// Console.WriteLine("Usuário editado com sucesso!");

// Editar usuário
usuarioService.EditarUsuario(
    1,
    "João Silva Atualizado",
    "joao.novo@email.com",
    "123456",
    "admin"
);

Console.WriteLine("Usuário editado com sucesso!");

// Listar usuários
List<Usuario> usuarios = usuarioService.ListarUsuarios();

foreach (Usuario usuario in usuarios)
{
    Console.WriteLine("----------------------");
    Console.WriteLine($"ID: {usuario.IdUsuario}");
    Console.WriteLine($"Nome: {usuario.Nome}");
    Console.WriteLine($"Email: {usuario.Email}");
    Console.WriteLine($"Tipo: {usuario.Tipo}");
}