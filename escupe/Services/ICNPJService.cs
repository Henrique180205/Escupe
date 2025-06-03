namespace escupe.Services
{
    public interface ICNPJService
    {
        bool ValidarCNPJ(string cnpj);
        Task<(bool valido, EmpresaInfo empresaInfo)> ValidarCNPJCompleto(string cnpj);
        bool VerificarCNPJExistente(string cnpj); 
    }


    public class EmpresaInfo
    {
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string SituacaoCadastral { get; set; }
        // Outros campos que você queira retornar
    }
}
