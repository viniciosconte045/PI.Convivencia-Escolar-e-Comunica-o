using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Xunit;

namespace SistemaCadastro
{
    // ==========================================
    // 1. ENTRADA DE DADOS (DTO)
    // ==========================================
    public class CadastroUsuarioDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF deve conter 11 dígitos numéricos ou estar formatado (000.000.000-00).")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [Compare(nameof(Senha), ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmacaoSenha { get; set; } = string.Empty;
    }

    // ==========================================
    // 2. VALIDAÇÕES DO CADASTRO
    // ==========================================
    public static class ValidadorCadastro
    {
        public static bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            cpf = Regex.Replace(cpf, @"[^\d]", "");
            if (cpf.Length != 11) return false;

            if (new string(cpf[0], 11) == cpf) return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }
    }

    // ==========================================
    // 3. CADASTRO SERVIÇO (REGRA DE NEGÓCIO + MYSQL)
    // ==========================================
    public interface ICadastroUsuarioServico
    {
        Task<(bool Sucesso, string Mensagem)> CadastrarUsuarioPublicoAsync(CadastroUsuarioDto dto);
    }

    public class CadastroUsuarioServico : ICadastroUsuarioServico
    {
        private readonly string _connectionString;

        public CadastroUsuarioServico(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<(bool Sucesso, string Mensagem)> CadastrarUsuarioPublicoAsync(CadastroUsuarioDto dto)
        {
            // Validação de CPF
            var cpfLimpo = Regex.Replace(dto.Cpf ?? "", @"[^\d]", "");
            if (!ValidadorCadastro.ValidarCpf(cpfLimpo))
            {
                return (false, "CPF informado é inválido.");
            }

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            // Validação de Duplicidade no MySQL
            const string queryVerificacao = "SELECT COUNT(1) FROM Usuarios WHERE Email = @Email OR Cpf = @Cpf";
            using (var cmdCheck = new MySqlCommand(queryVerificacao, connection))
            {
                cmdCheck.Parameters.AddWithValue("@Email", dto.Email.Trim().ToLower());
                cmdCheck.Parameters.AddWithValue("@Cpf", cpfLimpo);

                var existe = Convert.ToInt32(await cmdCheck.ExecuteScalarAsync()) > 0;
                if (existe)
                {
                    return (false, "Já existe um cadastro com este E-mail ou CPF.");
                }
            }

            // Hash da senha (BCrypt)
            string senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            // Persistência no MySQL
            const string queryInsert = @"
                INSERT INTO Usuarios (Nome, Email, Cpf, SenhaHash, DataCadastro, Ativo) 
                VALUES (@Nome, @Email, @Cpf, @SenhaHash, @DataCadastro, @Ativo);";

            using (var cmdInsert = new MySqlCommand(queryInsert, connection))
            {
                cmdInsert.Parameters.AddWithValue("@Nome", dto.Nome.Trim());
                cmdInsert.Parameters.AddWithValue("@Email", dto.Email.Trim().ToLower());
                cmdInsert.Parameters.AddWithValue("@Cpf", cpfLimpo);
                cmdInsert.Parameters.AddWithValue("@SenhaHash", senhaHash);
                cmdInsert.Parameters.AddWithValue("@DataCadastro", DateTime.UtcNow);
                cmdInsert.Parameters.AddWithValue("@Ativo", true);

                var linhasAfetadas = await cmdInsert.ExecuteNonQueryAsync();
                if (linhasAfetadas > 0)
                {
                    return (true, "Cadastro realizado com sucesso!");
                }
            }

            return (false, "Falha ao registrar o usuário no banco de dados.");
        }
    }

    // ==========================================
    // 4. CADASTRO PÚBLICO (CONTROLLER API)
    // ==========================================
    [ApiController]
    [Route("api/publico/[controller]")]
    public class AutocadastroController : ControllerBase
    {
        private readonly ICadastroUsuarioServico _cadastroServico;

        public AutocadastroController(ICadastroUsuarioServico cadastroServico)
        {
            _cadastroServico = cadastroServico;
        }

        [HttpPost]
        [AllowAnonymous] // Permite acesso sem necessidade de autenticação
        public async Task<IActionResult> Registrar([FromBody] CadastroUsuarioDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (sucesso, mensagem) = await _cadastroServico.CadastrarUsuarioPublicoAsync(dto);

            if (!sucesso)
            {
                return BadRequest(new { Mensagem = mensagem });
            }

            return Created(string.Empty, new { Mensagem = mensagem });
        }
    }

    // ==========================================
    // 5. TESTES DO CADASTRO (XUNIT)
    // ==========================================
    public class CadastroUsuarioTestes
    {
        private IList<ValidationResult> ValidarModelo(object modelo)
        {
            var resultados = new List<ValidationResult>();
            var contexto = new ValidationContext(modelo, null, null);
            Validator.TryValidateObject(modelo, contexto, resultados, true);
            return resultados;
        }

        [Theory]
        [InlineData("52998224725", true)]  // CPF Válido
        [InlineData("11111111111", false)] // Digitos repetidos
        [InlineData("12345678900", false)] // Dígitos verificadores inválidos
        [InlineData("abc", false)]         // Formato inválido
        public void ValidarCpf_DeveIdentificarSeCpfEValido(string cpf, bool esperado)
        {
            bool resultado = ValidadorCadastro.ValidarCpf(cpf);
            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public void Dto_SenhasDivergentes_DeveRetornarErroDeValidacao()
        {
            var dto = new CadastroUsuarioDto
            {
                Nome = "Usuário Teste",
                Email = "teste@dominio.com",
                Cpf = "52998224725",
                Senha = "SenhaForte123!",
                ConfirmacaoSenha = "OutraSenha123!"
            };

            var erros = ValidarModelo(dto);

            Assert.Contains(erros, e => e.ErrorMessage != null && e.ErrorMessage.Contains("não coincidem"));
        }

        [Fact]
        public void Dto_DadosCorretos_NaoDeveRetornarErros()
        {
            var dto = new CadastroUsuarioDto
            {
                Nome = "Usuário Teste",
                Email = "teste@dominio.com",
                Cpf = "52998224725",
                Senha = "SenhaForte123!",
                ConfirmacaoSenha = "SenhaForte123!"
            };

            var erros = ValidarModelo(dto);

            Assert.Empty(erros);
        }
    }
}