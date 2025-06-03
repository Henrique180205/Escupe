using System.ComponentModel.DataAnnotations;

namespace escupe.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Por favor, insira um email válido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

       // public string TipoUsuario { get; set; }

        [Display(Name = "Lembrar-me?")]
        public bool RememberMe { get; set; }
    }
}