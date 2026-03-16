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
            // 1. Garantir que o Email do Usuário seja ÚNICO no banco
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // 2. Configurar o relacionamento 1:N (Um usuário tem muitos livros)
            modelBuilder.Entity<Livro>()
                .HasOne(l => l.Usuario)
                .WithMany(u => u.Livros)
                .HasForeignKey(l => l.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade); // Se deletar o usuário, deleta os livros dele

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
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}