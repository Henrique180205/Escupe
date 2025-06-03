using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace escupe.Models
{
    [Table("Enderecos")]
    public class Endereco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(8)]
        [Column(TypeName = "char(8)")]
        public string? CEP { get; set; }

        [Required, StringLength(100)]
        public string? Logradouro { get; set; }

        [Required, StringLength(10)]
        public string? Numero { get; set; }

        [StringLength(100)]
        public string? Complemento { get; set; }

        [Required, StringLength(50)]
        public string? Bairro { get; set; }

        [Required, StringLength(50)]
        public string? Cidade { get; set; }

        [Required, StringLength(2)]
        [Column(TypeName = "char(2)")]
        public string? UF { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

        
    }
}
