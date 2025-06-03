using System.ComponentModel.DataAnnotations;

namespace escupe.DTOs
{
    public class UsuarioDto
    {
        // Campos para Cadastro e Login
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }

        // Campos usados APENAS no cadastro (não obrigatórios para login)
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        [DataType(DataType.Password)]
        public string ?ConfirmarSenha { get; set; } // Nullable para login

        [RegularExpression("^[CE]$", ErrorMessage = "Tipo deve ser 'C' (Candidato) ou 'E' (Empresa)")]
        public char TipoUsuario { get; set; } // Nullable para login

    }
}