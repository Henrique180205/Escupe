using escupe.Models;



public class Vaga
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string? Localizacao { get; set; }
    public decimal Salario { get; set; }
   public string Beneficios { get; set; }
    public DateTime DataPublicacao { get; set; }
    public int EmpresaId { get; set; } // Relacionamento com a empresa
    public Empresa Empresa { get; set; } // Propriedade de navegação
}
