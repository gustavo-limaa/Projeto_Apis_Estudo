using Microsoft.EntityFrameworkCore;

namespace Cadastro.Dados
{
    using Cadastro.Modelos;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Suas tabelas
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Livro> Livros { get; set; }

        // Aqui é onde configuramos as regras de negócio do banco (Fluent API)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
               .Property(u => u.Email)
                     .HasConversion(
                  e => e.Valor,              // Como vai pro banco (string)
                   v => new ValueEmail(v)          // Como volta do banco (Objeto Email)
                  ).HasMaxLength(255).IsRequired();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique(); // Garante que o email seja único

            // 2. Configurar o relacionamento 1:N (Um usuário tem muitos livros)
            modelBuilder.Entity<Livro>()
                .HasOne(l => l.Usuario)
                .WithMany(u => u.Livros)
                .HasForeignKey(l => l.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1. Configurar o Value Object de Preço no Livro
            modelBuilder.Entity<Livro>(entity =>
            {
                entity.OwnsOne(l => l.Preco, p =>
                {
                    p.Property(v => v.Valor)
                     .HasColumnName("Preco") // Nome da coluna no MySQL
                     .HasPrecision(18, 2)    // Importante para dinheiro!
                     .IsRequired();
                });
            }); // Se deletar o usuário, deleta os livros dele

            base.OnModelCreating(modelBuilder);
        }

        // 3. Mágica da Data de Criação Automática
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entradas = ChangeTracker.Entries()
                .Where(e => e.Entity is EntidadeBase && e.State == EntityState.Added);

            foreach (var entrada in entradas)
            {
                ((EntidadeBase)entrada.Entity).DataCriacao = DateTime.UtcNow;

                if (((EntidadeBase)entrada.Entity).Id == Guid.Empty)
                {
                    ((EntidadeBase)entrada.Entity).Id = Guid.NewGuid();
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}