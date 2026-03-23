namespace Prateleira_Universal.UseCases;

public class ObterProdutoPorIdUseCase
{
    private readonly IProdutoRepository _produtoRepository;

    public ObterProdutoPorIdUseCase(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<ProdutoBResponse?> ExecutarAsync(Guid id)
    {
        // 1. Busca o produto (pode vir nulo)
        var produto = await _produtoRepository.ObterPorIdAsync(id);

        // 2. Se for nulo, retorna nulo e deixa o Controller resolver (404)
        if (produto is null) return null;

        // 3. SEU MAPPER EM AÇÃO:
        // Não precisa de 'new' nem de instância do Mapper.
        // O método 'ToResponse()' já está disponível no objeto 'produto'.
        return produto.ToResponse();
    }
}