namespace escupe.Models
{
    public class Candidato
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string Localizacao { get; set; }
        public string AreaInteresse { get; set; } // Ex: "TI", "Vendas"
    }
}
