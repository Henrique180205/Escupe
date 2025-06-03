using System.Linq;
using escupe.Models;
using escupe.Data;
namespace escupe.Services
  

    {
        public class CPFService : ICPFService
        {

        private readonly ApplicationDbContext _context;

        public CPFService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool ValidarCPF(string cpf)
            {
                // Remove caracteres não numéricos
                cpf = new string(cpf.Where(char.IsDigit).ToArray());

                // Verifica se tem 11 dígitos
                if (cpf.Length != 11)
                    return false;

                // Algoritmo de validação do CPF
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                string tempCpf = cpf.Substring(0, 9);
                int soma = 0;

                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

                int resto = soma % 11;
                resto = resto < 2 ? 0 : 11 - resto;

                string digito = resto.ToString();
                tempCpf = tempCpf + digito;
                soma = 0;

                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

                resto = soma % 11;
                resto = resto < 2 ? 0 : 11 - resto;

                digito = digito + resto.ToString();

                return cpf.EndsWith(digito);
            }

        public bool VerificarCPFExistente(string cpf)
        {
            // Verifica se o CPF já existe no banco de dados
            return _context.Candidato.Any(c => c.CPF == cpf);
        }
    }
    }