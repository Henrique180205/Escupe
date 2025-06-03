namespace escupe.Services
{
    public class EnderecoViaCEP
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; } // Cidade
        public string UF { get; set; }         // Estado
        public bool Erro { get; set; }        // Campo "erro" no JSON da API
    }
}