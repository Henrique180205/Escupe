using escupe.Models;
using Microsoft.EntityFrameworkCore;

namespace escupe.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Candidato> Candidato { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Vaga> Vagas { get; set; }
    public DbSet<Candidatura> Candidatura { get; set; } // <-- Adicione esta linha

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vaga>()
            .HasOne(v => v.Empresa)
            .WithMany(e => e.Vagas)
            .HasForeignKey(v => v.EmpresaId);

        modelBuilder.Entity<Candidato>()
            .HasOne(c => c.Endereco)
            .WithMany()
            .HasForeignKey(c => c.EnderecoId);

        modelBuilder.Entity<Empresa>()
            .HasOne(e => e.Endereco)
            .WithMany()
            .HasForeignKey(e => e.EnderecoId);

        // Relacionamento Candidatura -> Vaga
        modelBuilder.Entity<Candidatura>()
            .HasOne(c => c.Vaga)
            .WithMany()
            .HasForeignKey(c => c.VagaId);

        // Relacionamento Candidatura -> Candidato
        modelBuilder.Entity<Candidatura>()
            .HasOne(c => c.Candidato)
            .WithMany()
            .HasForeignKey(c => c.CandidatoId);
    }
}
