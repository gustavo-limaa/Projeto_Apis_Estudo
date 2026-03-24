using Prateleira_Universal.Domain.Modelos.Enums;
using Prateleira_Universal.UseCases;
using Prateleira_Universal.UseCases.Dtos.ProdutoBaseDtos;

namespace Prateleira_universal.Testes.TesteUnitarios;

[Collection("Produto Collection")]
public class ProdutoTeste_POST
{
    private readonly ProdutoTestsFixture _fixture;

    public ProdutoTeste_POST(ProdutoTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CriarProduto_DeveChamarRepositorio_QuandoDadosForemValidos()
    {
        // 1. Arrange - Simples e sem erro de parâmetro
        var repoMock = new Mock<IProdutoRepository>();
        var useCase = new CriarProdutoUseCase(repoMock.Object);

        // Testando a lógica pura
        repoMock.Setup(r => r.SalvarAlteracoesAsync()).ReturnsAsync(true);

        // Usando sua Fixture para o Bogus
        var requestFake = _fixture.GerarRequestValido();

        // 2. Act
        var result = await useCase.ExecutarAsync(requestFake);

        // 3. Assert
        result.Should().NotBeNull();
        repoMock.Verify(r => r.AdicionarAsync(It.IsAny<Produto>()), Times.Once);
    }

    [Fact]
    public async Task CriarProduto_DeveRetornarNulo_QuandoSalvarNoBancoFalhar()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        var useCase = new CriarProdutoUseCase(repoMock.Object); // Testando o UseCase

        var requestFake = _fixture.GerarRequestValido();

        // Simulamos que o Adicionar funciona, mas o Salvar retorna FALSE
        repoMock.Setup(r => r.SalvarAlteracoesAsync()).ReturnsAsync(false);

        // --- ACT ---
        var result = await useCase.ExecutarAsync(requestFake);

        // --- ASSERT ---
        // Se o seu UseCase retorna null quando falha o salvamento:
        result.Should().BeNull();

        // Verificamos se ele ao menos tentou adicionar antes de falhar o save
        repoMock.Verify(r => r.AdicionarAsync(It.IsAny<Produto>()), Times.Once);
    }

    [Fact]
    public async Task CriarProduto_DeveLancarExcecao_QuandoRequestForNulo()
    {
        var repoMock = new Mock<IProdutoRepository>();
        var usecase = new CriarProdutoUseCase(repoMock.Object);

        // No FluentAssertions, para testar exceção em métodos Async, fazemos assim:
        Func<Task> act = async () => await usecase.ExecutarAsync(null);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CriarProduto_DeveRepassarExcecao_QuandoRepositorioFalhar()
    {
        var repoMock = new Mock<IProdutoRepository>();
        var usecase = new CriarProdutoUseCase(repoMock.Object);
        var request = _fixture.GerarRequestValido();

        repoMock.Setup(r => r.AdicionarAsync(It.IsAny<Produto>()))
                .ThrowsAsync(new Exception("Erro de banco"));

        Func<Task> act = async () => await usecase.ExecutarAsync(request);

        await act.Should().ThrowAsync<Exception>().WithMessage("Erro de banco");
    }

    [Fact]
    public async Task CriarProduto_DeveRetornarBadRequest_QuandoRequestForInvalido()
    {
        var repoMock = new Mock<IProdutoRepository>();
        var usecase = new CriarProdutoUseCase(repoMock.Object);
        var request = _fixture.GerarRequestComErro("nome-vazio");
        Func<Task> act = async () => await usecase.ExecutarAsync(request);

        await act.Should().ThrowAsync<ArgumentException>()
              .WithMessage("*Nome*");
    }
}