
namespace escupe.Models
{

    public class Empresa
    {
        public int Id { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set; }
        public string Telefone { get; set; }
        public string? Site { get; set; }
        public string TipoUsuario { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string? AreaAtuacao { get; set; }

        // Relacionamento com Endereco
        public int EnderecoId { get; set; }
        public Endereco Endereco { get; set; }
        // Relacionamento com Vagas
        public ICollection<Vaga> Vagas { get; set; }
    }
}