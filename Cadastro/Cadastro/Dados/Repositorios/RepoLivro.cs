using Cadastro.Dados;
using Cadastro.Intefaces;
using Cadastro.Modelos;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Dados.Repositorios
{
    public class RepoLivro : IRepositorioLivros
    {
        private readonly AppDbContext _context;

        public RepoLivro(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AdicionarAsync(Livro livro)
        {
            ArgumentNullException.ThrowIfNull(livro);

            await _context.Livros.AddAsync(livro);

            // O SaveChangesAsync retorna o número de linhas alteradas no banco
            int linhasAfetadas = await _context.SaveChangesAsync();

            // Se for maior que 0, significa que o registro foi criado!
            return linhasAfetadas > 0;
        }

        public async Task<bool> AtualizarAsync(Livro livro)
        {
            ArgumentNullException.ThrowIfNull(livro);

            // 1. Verificamos se o livro existe no banco (rastreamento)
            var livroExistente = await _context.Livros.FirstOrDefaultAsync(l => l.Id == livro.Id);

            if (livroExistente == null)
            {
                return false; // Ou lançar a sua NotFoundException, dependendo da sua estratégia
            }

            // 2. Atualizamos as propriedades (ou usamos o Mapper que discutimos)
            _context.Entry(livroExistente).CurrentValues.SetValues(livro);

            // 3. Persistimos e retornamos se houve alteração real
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            if (id == Guid.Empty) return false;

            var livroExistente = await _context.Livros.FirstOrDefaultAsync(l => l.Id == id);

            if (livroExistente == null) return false;

            _context.Livros.Remove(livroExistente);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Livro?> ObterPorIdAsync(Guid id)
        {
            if (id == Guid.Empty) return null;

            return await _context.Livros.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Livro>> ObterTodosAsync()
        {
            // O EF já retorna uma lista vazia [] se não houver dados.
            return await _context.Livros.AsNoTracking().ToListAsync();
        }
    }
}