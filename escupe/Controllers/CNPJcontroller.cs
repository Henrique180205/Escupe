using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace escupe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CNPJController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CNPJController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Ação para validar CNPJ
        [HttpGet("{cnpj}")]
        public async Task<IActionResult> ValidarCNPJ(string cnpj)
        {
            // Remove caracteres especiais do CNPJ (como ponto, barra e hífen)
            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

            // Verifica se o CNPJ tem o formato correto
            if (cnpj.Length != 14)
            {
                return BadRequest("CNPJ inválido.");
            }

            // Fazendo a requisição para a API da ReceitaWS
            var response = await _httpClient.GetStringAsync($"https://www.receitaws.com.br/v1/cnpj/{cnpj}");

            if (string.IsNullOrEmpty(response))
            {
                return NotFound("CNPJ não encontrado.");
            }

            // Retorna a resposta da API para o cliente
            return Ok(response);
        }
    }
}
