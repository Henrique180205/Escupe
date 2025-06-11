using System.ComponentModel.DataAnnotations;
namespace escupe.ViewModels
{
    public class CadastroEmpresaViewModel
    {
        [Required(ErrorMessage = "O nome fantasia é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome fantasia deve ter no máximo 100 caracteres.")]
        public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "A razão social é obrigatória.")]
        [StringLength(150, ErrorMessage = "A razão social deve ter no máximo 150 caracteres.")]
        public string RazaoSocial { get; set; }
        public string TipoUsuario { get; set; }
        public string AreaAtuacao { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve conter 14 dígitos.")]
        public string CNPJ { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
       
        public string Telefone { get; set; }

        // Endereço
        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        public string Numero { get; set; }

        public string Complemento { get; set; }

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
        public string confSenha {get; set;}

    }
}