namespace escupe.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Ramo { get; set; } // Ex: "Tecnologia", "Varejo"
    }
}
