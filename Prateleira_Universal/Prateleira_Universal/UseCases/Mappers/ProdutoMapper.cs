namespace Prateleira_Universal.UseCases.Mappers;

public static class ProdutoMapper
{
    public static Produto ToEntity(this ProdutoBRequest dto)
    {
        return new Produto
        {
            ProdutoID = Guid.NewGuid(),
            Nome = dto.Nome,
            Tipo = dto.Tipo,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            Especificacoes = dto.Especificacoes,
        };
    }

    public static ProdutoBResponse ToResponse(this Produto pdt)
    {
        return new ProdutoBResponse(pdt.ProdutoID
            , pdt.Nome
            , pdt.Descricao,
            pdt.Tipo,
            pdt.Preco,
            pdt.Especificacoes);
    }

    public static void UpdateFromDto(this Produto p, ProdutoBUpdate dto)
    {
        p.Nome = dto.Nome;
        p.Descricao = dto.Descricao;
        p.Tipo = dto.Tipo;
        p.Preco = dto.Preco;
        p.Especificacoes = dto.Especificacoes;
    }
}