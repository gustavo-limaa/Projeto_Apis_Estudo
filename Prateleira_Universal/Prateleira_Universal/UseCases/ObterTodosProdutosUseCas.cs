using Prateleira_Universal.UseCases.Dtos.ProdutoBaseDtos;
using Prateleira_Universal.UseCases.Mappers;

namespace Prateleira_Universal.UseCases;

public class ObterTodosProdutosUseCase
{
    private readonly IProdutoRepository _produtoRepository;

    public ObterTodosProdutosUseCase(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<IEnumerable<ProdutoBResponse>> ExecutarAsync()
    {
        // 1. Busca os dados puros do Repositório
        var produtos = await _produtoRepository.ObterTodosAsync();

        // 2. Usa o seu Mapper (Método de Extensão) para transformar a lista
        // Aqui o Select brilha!
        return produtos.Select(p => p.ToResponse()).ToList();
    }
}