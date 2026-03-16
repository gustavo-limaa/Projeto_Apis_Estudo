using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Prateleira_Universal.Dtos;
using System.Collections.Generic;

namespace Prateleira_Universal.Data.context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Definimos como o EF deve comparar os dicionários na memória
        var comparer = new ValueComparer<Dictionary<string, string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToDictionary(entry => entry.Key, entry => entry.Value)
        );

        // 2. Aplicamos a conversão para JSON e o comparador de valor
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.Property(p => p.Especificacoes)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new Dictionary<string, string>()
                )
                .Metadata.SetValueComparer(comparer); // Agora usando o nome correto: modelBuilder
        });
    }
}