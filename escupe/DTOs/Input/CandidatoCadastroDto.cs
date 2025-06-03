using System.ComponentModel.DataAnnotations;

namespace escupe.DTOs.Input
{
    public class CandidatoCadastroDto : UsuarioDto
    {
        // ... outros campos ...

        [Required(ErrorMessage = "CPF é obrigatório")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter 11 dígitos")]
        public string? CPF { get; set; }

        [Required(ErrorMessage = "CEP é obrigatório")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "CEP deve conter 8 dígitos")]
        public string? CEP { get; set; }

        public string? NomeCompleto { get; set; }

        // Campos que serão preenchidos após validação
        public int Numero { get; set; }
        public string? Logradouro { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? UF { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [Phone(ErrorMessage = "O telefone informado não é válido")]
        public string? Telefone { get; set; } // Novo campo de telefone

        [Required(ErrorMessage = "Endereço é obrigatório")]
        public EnderecoDto? Endereco { get; set; }
    }
}
