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

        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException("O nome do produto é obrigatório.", nameof(dto.Nome));

        var entity = dto.ToEntity();

        await _Repo.AdicionarAsync(entity);

        var salvar = await _Repo.SalvarAlteracoesAsync();
        if (!salvar) return null;

        return entity.ToResponse();
    }
}