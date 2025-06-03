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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vaga>()
            .HasOne(v => v.Empresa)
            .WithMany(e => e.Vagas)
            .HasForeignKey(v => v.EmpresaId);

        // Relacionamento 1:1 entre Usuario e Candidato
        // modelBuilder.Entity<Usuario>()
        //     .HasOne(u => u.Candidato)
        //     .WithOne(c => c.Usuario)
        //     .HasForeignKey<Candidato>(c => c.UsuarioId);

        // Relacionamento 1:1 entre Usuario e Empresa
        // modelBuilder.Entity<Usuario>()
        //     .HasOne(u => u.Empresa)
        //     .WithOne(e => e.Usuario)
        //     .HasForeignKey<Empresa>(e => e.UsuarioId);

        // Relacionamento opcional entre Candidato e Endereco
        modelBuilder.Entity<Candidato>()
            .HasOne(c => c.Endereco)
            .WithMany()
            .HasForeignKey(c => c.EnderecoId);

        // Relacionamento opcional entre Empresa e Endereco
        modelBuilder.Entity<Empresa>()
            .HasOne(e => e.Endereco)
            .WithMany()
            .HasForeignKey(e => e.EnderecoId);
    }
}
