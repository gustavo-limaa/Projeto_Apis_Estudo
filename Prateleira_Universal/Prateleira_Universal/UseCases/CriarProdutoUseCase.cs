using Prateleira_Universal.UseCases.Mappers;

namespace Prateleira_Universal.UseCases;

public class CriarProdutoUseCase
{
    private readonly IProdutoRepository _Repo;

    public CriarProdutoUseCase(IProdutoRepository produtoRepository)
    {
        _Repo = produtoRepository;
    }

    public async Task<ProdutoBResponse> ExecutarAsync(ProdutoBRequest dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var entity = dto.ToEntity();

        await _Repo.AdicionarAsync(entity);
        await _Repo.SalvarAlteracoesAsync();

        return entity.ToResponse();
    }
}