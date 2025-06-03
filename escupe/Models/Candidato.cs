using escupe.Models;

public class Candidato
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; }
    public string CPF { get; set; }
    public string Telefone { get; set; }


    public string TipoUsuario { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
   

    // Relacionamento com Endereco
    public int? EnderecoId { get; set; }
    public Endereco Endereco { get; set; }
}