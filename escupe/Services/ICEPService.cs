using System.Threading.Tasks;

namespace escupe.Services
{
    public interface ICEPService
    {
        Task<(bool valido, EnderecoViaCEP endereco)> ValidarCEP(string cep);
    }
}
