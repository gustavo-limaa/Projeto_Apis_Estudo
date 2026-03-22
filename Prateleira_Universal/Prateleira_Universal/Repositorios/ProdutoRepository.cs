using Prateleira_Universal.interfaces;

namespace Prateleira_Universal.Repositorios;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> ObterTodosAsync()
        => await _context.Produtos.AsNoTracking().ToListAsync();

    public async Task<Produto?> ObterPorIdAsync(Guid id)
        => await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoID == id);

    public async Task AdicionarAsync(Produto produto)
        => await _context.Produtos.AddAsync(produto);

    public async Task AtualizarAsync(Produto produto)
        => _context.Produtos.Update(produto);

    public async Task DeletarAsync(Produto produto)
        => _context.Produtos.Remove(produto);

    public async Task<IEnumerable<Produto>> ObterPorCategoriaAsync(EnumCategoria categoria)
    {
        return await _context.Produtos
            .Where(p => p.Tipo == categoria)
            .ToListAsync();
    }

    public async Task<bool> SalvarAlteracoesAsync()
        => await _context.SaveChangesAsync() > 0;
}