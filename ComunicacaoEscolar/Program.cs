using ComunicacaoEscolar.Services;

UsuarioService usuarioService = new UsuarioService();

usuarioService.CadastrarUsuario(
    "João Silva",
    "joao@email.com",
    "123456",
    "admin"
);

Console.WriteLine("Usuário cadastrado com sucesso!");