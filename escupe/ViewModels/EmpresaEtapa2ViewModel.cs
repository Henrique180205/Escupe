using System.ComponentModel.DataAnnotations;
namespace escupe.ViewModels 
{
    public class EmpresaEtapa2ViewModel
    {
        [Required(ErrorMessage = "A razão social é obrigatória.")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "O nome fantasia é obrigatório.")]
        public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(14, ErrorMessage = "O CNPJ deve ter 14 dígitos.")]
        public string CNPJ { get; set; }

        [Required(ErrorMessage = "A área de atuação é obrigatória.")]
        public string AreaAtuacao { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(8, ErrorMessage = "O CEP deve ter 8 dígitos.")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        public string Numero { get; set; }

        public string Complemento { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(2, ErrorMessage = "O estado deve ter 2 letras.")]
        public string UF { get; set; }
    }
}