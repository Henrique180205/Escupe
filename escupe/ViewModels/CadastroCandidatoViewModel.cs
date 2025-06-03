using System.ComponentModel.DataAnnotations;

namespace escupe.ViewModels
{
    public class CadastroCandidatoViewModel
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome completo deve ter no máximo 100 caracteres.")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"\d{3}\.\d{3}\.\d{3}-\d{2}", ErrorMessage = "O CPF deve estar no formato 000.000.000-00.")]
        public string CPF { get; set; }

        public DateTime DataNascimento { get; set; }

        // Endereço
        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        public string Numero { get; set; }
        public string TipoUsuario { get; set; }
        public string? Complemento { get; set; } // Permitir valores nulos

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O estado (UF) é obrigatório.")]
        [StringLength(2, ErrorMessage = "A UF deve ter 2 caracteres.")]
        public string UF { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
       
        public string CEP { get; set; }

        // Campos adicionais para transferência de dados
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
    }
}

