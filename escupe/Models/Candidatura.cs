using System;

namespace escupe.Models
{
    public class Candidatura
    {
        public int Id { get; set; }
        public int VagaId { get; set; }
        public Vaga Vaga { get; set; }
        public int CandidatoId { get; set; }
        public Candidato Candidato { get; set; }
        public DateTime DataCandidatura { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }           // Novo campo
        public string Telefone { get; set; }        // Novo campo
        public string? CurriculoPath { get; set; }  // Novo campo
        public string Descricao { get; set; }       // Novo campo
    }
}

