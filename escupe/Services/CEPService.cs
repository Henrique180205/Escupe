using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace escupe.Services
{
    public class CEPService : ICEPService
    {
        private readonly HttpClient _httpClient;

        public CEPService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool valido, EnderecoViaCEP endereco)> ValidarCEP(string cep)
        {
            // Validação básica do formato do CEP
            if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8 || !long.TryParse(cep, out _))
                return (false, null);

            try
            {
                // Faz a requisição para a API ViaCEP
                var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
                if (!response.IsSuccessStatusCode)
                    return (false, null);

                var content = await response.Content.ReadAsStringAsync();
                var endereco = JsonSerializer.Deserialize<EnderecoViaCEP>(content);

                // Verifica se o campo "erro" está presente no JSON
                if (endereco == null || endereco.Erro)
                    return (false, null);

                return (true, endereco);
            }
            catch
            {
                return (false, null);
            }
        }
    }
}
