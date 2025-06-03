// C#
namespace escupe.ViewModels
{
    public class VagaViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string? Localizacao { get; set; }
        public decimal Salario { get; set; }
        public string Beneficios { get; set; }
        public string Setor { get; set; }
        public DateTime DataPublicacao { get; set; }
        public int EmpresaId { get; set; }
    }

    public class FeedVagaViewModel
    {
        public List<VagaViewModel> Vagas { get; set; } = new();
        public VagaViewModel? VagaSelecionada { get; set; }
        public string? TermoBusca { get; set; }
        public string? LocalizacaoBusca { get; set; }
    }
}
