using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace escupe.Controllers
{
    [ApiController] // Indica que é um controller de API
    [Route("api/[controller]")] // Rota base: /api/cep
    public class CepController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CepController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Consulta um endereço pelo CEP.
        /// </summary>
        /// <param name="cep">CEP (8 dígitos, sem hífen).</param>
        /// <returns>Dados do endereço ou mensagem de erro.</returns>
        [HttpGet("{cep}")]
        public async Task<IActionResult> ConsultarCEP(string cep)
        {     // Validação do CEP (código completo dentro do método)
            if (string.IsNullOrWhiteSpace(cep))
            {
                return BadRequest("CEP não pode ser vazio.");
            }

            if (cep.Length != 8 || !cep.All(char.IsDigit))
            {
                return BadRequest("CEP deve ter 8 dígitos numéricos.");
            }

            try
            {
                // Consulta a API ViaCEP
                var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

                if (!response.IsSuccessStatusCode)
                    return NotFound("CEP não encontrado.");

                var content = await response.Content.ReadAsStringAsync();
                var endereco = JsonSerializer.Deserialize<EnderecoViaCEP>(content);

                // Verifica se o CEP é inválido (ViaCEP retorna campo "erro": true)
                if (endereco == null || endereco.Erro)
                    return NotFound("CEP inválido.");

                // Retorna os dados do endereço
                return Ok(endereco);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Falha ao consultar a API ViaCEP: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return StatusCode(500, $"Erro ao processar resposta da API: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }

    // Modelo para a resposta da ViaCEP
    public class EnderecoViaCEP
    {
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string? Complemento { get; set; } // Opcional (pode ser null)
        public string Bairro { get; set; } = string.Empty;
        public string Localidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public bool Erro { get; set; }
    }
}