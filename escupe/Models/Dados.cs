namespace escupe.Models
{
    public static class Dados
    {
        public static List<Candidato> Candidatos = new List<Candidato>
    {
            // dados ficticios para Candidado
        new Candidato { Id = 1, Nome = "Ana Souza", Email = "ana@teste.com", Senha = "123", AreaInteresse = "TI" },
        new Candidato { Id = 2, Nome = "Carlos Lima", Email = "carlos@teste.com", Senha = "123", AreaInteresse = "Vendas" }
    };

        public static List<Empresa> Empresas = new List<Empresa>
    {
            //dados ficticios para Empresa
        new Empresa { Id = 1, Nome = "Tech Solutions", CNPJ = "00.000.000/0001-00", Email = "contato@tech.com", Senha = "123", Ramo = "TI" }
    };
    }
}
