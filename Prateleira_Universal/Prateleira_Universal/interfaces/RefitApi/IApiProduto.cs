using Refit;

namespace Prateleira_Universal.interfaces.RefitApi;

public interface IApiProduto
{
    [Get("/api/produtos/{id}")]
    Task<ProdutoBResponse> ObterTodosPorID(Guid id);

    [Post("/api/produtos")]
    Task<ProdutoBResponse> CriarProduto([Body] ProdutoBRequest novoProduto);

    [Delete("/api/produtos/{id}")]
    Task<ApiResponse<object>> DeletarProdutoPorID(Guid id);

    [Put("/api/produtos/{id}")]
    Task AtualizarProdutoPorID(Guid id, [Body] ProdutoBUpdate dadosAtt);

    [Get("/api/produtos")]
    Task<List<ProdutoBResponse>> ObterPorCategoria([Query] EnumCategoria categoria);
}