using Prateleira_Universal.Modelos.Enums;

namespace Prateleira_universal.Testes.TesteUnitarios;

[Collection("Produto Collection")]
public class ProdutoTeste_POST
{
    [Fact]
    private async Task CriarProduto_DeveRetornarCreated_QuandoDadosForemValidos()
    {
        // --- ARRANGE (Preparar) ---
        var repoMock = new Mock<IProdutoRepository>();

        // Bogus configurado com a ordem exata do seu construtor
        var requestFake = new Faker<ProdutoBRequest>()
            .CustomInstantiator(f => new ProdutoBRequest(
                f.Commerce.ProductName(),                     // 1. nome (string)
                f.Commerce.ProductDescription(),               // 2. descricao (string)
                EnumCategoria.Erva_Mate,                       // 3. tipo (Enum)
                new Dictionary<string, string> { { "Sabor", "Menta" } }, // 4. especificacoes (Dictionary)
                f.Finance.Amount(10, 100)                      // 5. preco (decimal)
            )).Generate();

        // Configuramos o Mock para dizer que o salvamento deu certo
        repoMock.Setup(r => r.SalvarAlteracoesAsync()).ReturnsAsync(true);

        var controller = new ProdutoController(repoMock.Object);

        // --- ACT (Agir) ---
        var result = await controller.CriarProdutoAsync(requestFake);

        // --- ASSERT (Verificar) ---
        var createdResult = result as CreatedAtActionResult;

        // Verificações com FluentAssertions
        createdResult.Should().NotBeNull();
        createdResult.StatusCode.Should().Be(201);

        var response = createdResult.Value as ProdutoBResponse;
        response.Should().NotBeNull();
        response.Nome.Should().Be(requestFake.Nome);

        // Verificamos se o Repositório foi chamado para adicionar
        repoMock.Verify(r => r.AdicionarAsync(It.IsAny<Produto>()), Times.Once);
    }

    [Fact]
    public async Task CriarProduto_DeveRetornarBadRequest_QuandoSalvarFalhar()
    {
        // --- ARRANGE (Preparar) ---
        var repoMock = new Mock<IProdutoRepository>();
        var requestFake = new Faker<ProdutoBRequest>()
            .CustomInstantiator(f => new ProdutoBRequest(
                f.Commerce.ProductName(),
                f.Commerce.ProductDescription(),
                EnumCategoria.Erva_Mate,
                new Dictionary<string, string> { { "Sabor", "Menta" } },
                f.Finance.Amount(10, 100)
            )).Generate();

        // Configuramos o Mock para dizer que o salvamento falhou

        repoMock.Setup(r => r.SalvarAlteracoesAsync()).ReturnsAsync(false);
        var controller = new ProdutoController(repoMock.Object);

        // --- ACT (Agir) ---
        var result = await controller.CriarProdutoAsync(requestFake);

        // --- ASSERT (Verificar) ---
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task CriarProduto_DeveRetornarBadRequest_QuandoRequestForNulo()
    {
        // --- ARRANGE (Preparar) ---
        var repoMock = new Mock<IProdutoRepository>();
        var controller = new ProdutoController(repoMock.Object);

        // --- ACT (Agir) ---
        var result = await controller.CriarProdutoAsync(null);

        // --- ASSERT (Verificar) ---
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task CriarProduto_DeveRetornarStatus500_QuandoOcorreExcecao()
    {
        // --- ARRANGE (Preparar) ---
        var repoMock = new Mock<IProdutoRepository>();
        var requestFake = new Faker<ProdutoBRequest>()
            .CustomInstantiator(f => new ProdutoBRequest(
                f.Commerce.ProductName(),
                f.Commerce.ProductDescription(),
                EnumCategoria.Erva_Mate,
                new Dictionary<string, string> { { "Sabor", "Menta" } },
                f.Finance.Amount(10, 100)
            )).Generate();
        // Configuramos o Mock para lançar uma exceção
        repoMock.Setup(r => r.AdicionarAsync(It.IsAny<Produto>())).ThrowsAsync(new Exception("Erro de teste"));
        var controller = new ProdutoController(repoMock.Object);
        // --- ACT (Agir) ---
        var result = await controller.CriarProdutoAsync(requestFake);
        // --- ASSERT (Verificar) ---
        var statusResult = result as ObjectResult;
        statusResult.Should().NotBeNull();
        statusResult.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task CriarProduto_DeveRetornarBadRequest_QuandoRequestForInvalido()
    {
        // --- ARRANGE (Preparar) ---
        var repoMock = new Mock<IProdutoRepository>();
        var controller = new ProdutoController(repoMock.Object);
        // Criamos um request com dados inválidos (ex: nome vazio)
        var requestInvalido = new ProdutoBRequest(
            "", // Nome vazio
            "Descrição válida",
            EnumCategoria.Erva_Mate,
            new Dictionary<string, string> { { "Sabor", "Menta" } },
            50m
        );
        // --- ACT (Agir) ---
        var result = await controller.CriarProdutoAsync(requestInvalido);
        // --- ASSERT (Verificar) ---
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
    }
}