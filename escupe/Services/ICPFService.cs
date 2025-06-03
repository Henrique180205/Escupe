namespace escupe.Services
{
    public interface ICPFService
    {
        bool ValidarCPF(string cpf);
        bool VerificarCPFExistente(string cpf);
    }
}