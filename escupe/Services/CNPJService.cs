using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using escupe.Services;
using System.Linq;
using escupe.Data;

namespace escupe.Services
{
    public class CNPJService :ICNPJService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CNPJService> _logger;

        public CNPJService(ApplicationDbContext context, HttpClient httpClient, ILogger<CNPJService> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;

            // Configurações básicas do HttpClient
            _httpClient.BaseAddress = new Uri("https://www.receitaws.com.br/v1/");
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public bool ValidarCNPJ(string cnpj)
        {
            try
            {
                cnpj = LimparCNPJ(cnpj);

                if (cnpj.Length != 14)
                    return false;

                // Algoritmo de validação de CNPJ
                int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

                string tempCnpj = cnpj.Substring(0, 12);
                int soma = 0;

                for (int i = 0; i < 12; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

                int resto = (soma % 11);
                resto = resto < 2 ? 0 : 11 - resto;

                string digito = resto.ToString();
                tempCnpj = tempCnpj + digito;
                soma = 0;

                for (int i = 0; i < 13; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

                resto = (soma % 11);
                resto = resto < 2 ? 0 : 11 - resto;

                digito = digito + resto.ToString();

                return cnpj.EndsWith(digito);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar CNPJ");
                return false;
            }
        }
        public bool VerificarCNPJExistente(string cnpj)
        {
            // Verifica se o CNPJ já existe no banco de dados
            return _context.Empresas.Any(e => e.CNPJ == cnpj);
        }
        public async Task<(bool valido, EmpresaInfo empresaInfo)> ValidarCNPJCompleto(string cnpj)
        {
            try
            {
                cnpj = LimparCNPJ(cnpj);

                if (!ValidarCNPJ(cnpj))
                    return (false, null);

                var response = await _httpClient.GetAsync($"cnpj/{cnpj}");

                if (!response.IsSuccessStatusCode)
                    return (false, null);

                var content = await response.Content.ReadAsStringAsync();
                var empresaData = JsonSerializer.Deserialize<ReceitaWSResponse>(content);

                if (empresaData == null || empresaData.Status == "ERROR")
                    return (false, null);

                return (true, new EmpresaInfo
                {
                    RazaoSocial = empresaData.Nome,
                    NomeFantasia = empresaData.Fantasia,
                    SituacaoCadastral = empresaData.Situacao
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar CNPJ na ReceitaWS");
                return (false, null);
            }
        }

        private string LimparCNPJ(string cnpj)
        {
            return new string(cnpj.Where(char.IsDigit).ToArray());
        }

        private class ReceitaWSResponse
        {
            public string Nome { get; set; }
            public string Fantasia { get; set; }
            public string Situacao { get; set; }
            public string Status { get; set; }
            // Outros campos que você queira mapear
        }
    }
}