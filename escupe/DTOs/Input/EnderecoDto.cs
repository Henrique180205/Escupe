using escupe.DTOs;
using System.ComponentModel.DataAnnotations;

public class EnderecoDto:UsuarioDto
{
    [Required(ErrorMessage = "CEP é obrigatório")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "CEP inválido")]
    public string? CEP { get; set; }

    [Required(ErrorMessage = "Logradouro é obrigatório")]
    [StringLength(100, ErrorMessage = "Máximo de 100 caracteres")]
    public string ?Logradouro { get; set; }

    [Required(ErrorMessage = "Número é obrigatório")]
    [StringLength(10, ErrorMessage = "Máximo de 10 caracteres")]
    public string ?Numero { get; set; }

    [StringLength(100, ErrorMessage = "Máximo de 100 caracteres")]
    public string? Complemento { get; set; }

    [Required(ErrorMessage = "Bairro é obrigatório")]
    [StringLength(50, ErrorMessage = "Máximo de 50 caracteres")]
    public string? Bairro { get; set; }

    [Required(ErrorMessage = "Cidade é obrigatório")]
    [StringLength(50, ErrorMessage = "Máximo de 50 caracteres")]
    public string ?Cidade { get; set; }

    [Required(ErrorMessage = "UF é obrigatório")]
    [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "UF inválida")]
    public string? UF { get; set; }
}