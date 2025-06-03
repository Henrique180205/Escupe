using System.ComponentModel.DataAnnotations;

namespace escupe.DTOs.Input
{
    public class EmpresaCadastroDto:UsuarioDto

    {
        
        [Required(ErrorMessage = "Razão social obrigatória")]
        public string ?RazaoSocial { get; set; }

        [Required(ErrorMessage = "CNPJ obrigatório")]
        [StringLength(18, MinimumLength = 14, ErrorMessage = "CNPJ inválido")]
        public string? CNPJ { get; set; }

        public string? Site { get; set; }

        // Outros campos específicos da empresa
        public string? NomeFantasia { get; set; }
        public string? SetorAtuacao { get; set; }

        [Required(ErrorMessage = "Endereço é obrigatório")]
        public EnderecoDto? Endereco { get; set; }
    }
}