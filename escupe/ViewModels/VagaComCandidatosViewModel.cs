public class VagaComCandidatosViewModel
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Localizacao { get; set; }
    public decimal Salario { get; set; }
    public string Beneficios { get; set; }
    public DateTime DataPublicacao { get; set; }
    public List<CandidatoViewModel> Candidatos { get; set; }
}

public class CandidatoViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string TituloProfissional { get; set; }
    public DateTime DataCandidatura { get; set; }
    public string Status { get; set; }
}
