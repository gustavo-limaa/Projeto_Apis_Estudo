using Prateleira_Universal.Modelos.Enums;
using Xunit; // Necessário para os atributos de Teste

namespace Prateleira_universal.Testes;

// 1. ESTA É A FIXTURE (A Fábrica)
public class ProdutoTestsFixture
{
    public Mock<IProdutoRepository> RepoMock { get; private set; }

    public ProdutoTestsFixture()
    {
        RepoMock = new Mock<IProdutoRepository>();
    }

    public List<Produto> GerarListaValida(int quantidade)
    {
        return new Faker<Produto>()
            .CustomInstantiator(f => new Produto(
                Guid.NewGuid(),
                f.Commerce.ProductName(),
                EnumCategoria.Erva_Mate,
                f.Commerce.ProductDescription(),
                f.Finance.Amount(10, 100),
                new Dictionary<string, string> { { "Sabor", "Menta" } }
            )).Generate(quantidade);
    }

    public ProdutoBRequest GerarRequestValido()
    {
        var f = new Faker();
        return new ProdutoBRequest(
            f.Commerce.ProductName(),                 // 1. Nome (string)
            f.Commerce.ProductDescription(),          // 2. Descricao (string)
            EnumCategoria.Erva_Mate,                  // 3. Tipo (Enum)
            new Dictionary<string, string> { { "Origem", "MS" } }, // 4. Especificacoes (Dict)
            f.Finance.Amount(10, 100)                 // 5. Preco (decimal)
        );
    }
}

// 2. ESTA É A DEFINIÇÃO DA COLEÇÃO (A Etiqueta)
[CollectionDefinition("Produto Collection")]
public class ProdutoFixCollection : ICollectionFixture<ProdutoTestsFixture>
{
    // Não precisa de código aqui dentro!
}