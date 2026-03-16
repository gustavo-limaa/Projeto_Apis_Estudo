using FilmeApis.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace FilmeApis.Data;


public class FilmeContext : DbContext
{
    public FilmeContext(DbContextOptions<FilmeContext> options) : base(options)
    {
    }
    public DbSet<Filme> Filmes { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Endereco> Endereços { get; set; }
    public DbSet<Sessao> Sessoes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Relacionamento Cinema <-> Endereço (1:1)
        modelBuilder.Entity<Cinema>()
            .HasOne(cinema => cinema.Endereco)
            .WithOne(endereco => endereco.Cinema)
            .HasForeignKey<Cinema>(cinema => cinema.EnderecoId)
            .OnDelete(DeleteBehavior.Restrict); // Proteção: impede deletar endereço com cinema

        // 2. Relacionamento Sessão <-> Filme (N:1)
        modelBuilder.Entity<Sessao>()
            .HasOne(sessao => sessao.Filme)
            .WithMany(filme => filme.Sessoes)
            .HasForeignKey(sessao => sessao.FilmeId);

        // 3. Relacionamento Sessão <-> Cinema (N:1)
        modelBuilder.Entity<Sessao>()
            .HasOne(sessao => sessao.Cinema)
            .WithMany(cinema => cinema.Sessoes)
            .HasForeignKey(sessao => sessao.CinemaId);
    }
}
