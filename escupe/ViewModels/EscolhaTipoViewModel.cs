using System.ComponentModel.DataAnnotations;

namespace escupe.ViewModels
{
    public class EscolhaTipoViewModel
    {
        [Required(ErrorMessage = "Selecione um tipo de cadastro")]
        public char TipoUsuario { get; set; } // 'C' ou 'E'

      
    }
}