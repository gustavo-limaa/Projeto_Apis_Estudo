using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_universal.Testes.TesteUnitarios;

[Collection("Produto Collection")]
public class TesteProduto_GET
{
    [Fact]
    public async Task ObterTodos_DeveRetornarOk_QuandoExistiremProdutos()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        var produtosFake = new ProdutoTestsFixture().GerarListaValida(5);

        repoMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(produtosFake);
        var controller = new ProdutoController(repoMock.Object);

        // --- ACT ---
        var result = await controller.ObterTodosAsync();

        // --- ASSERT ---
        // Em vez de usar "as", usamos o FluentAssertions para validar o tipo
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);

        var responseList = okResult.Value.Should().BeAssignableTo<IEnumerable<ProdutoBResponse>>().Subject;
        responseList.Should().HaveCount(produtosFake.Count);
    }

    [Fact]
    public async Task ObterTodos_DeveRetornarNoContent_QuandoNaoExistiremProdutos()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(new List<Produto>());
        var controller = new ProdutoController(repoMock.Object);
        // --- ACT ---
        var result = await controller.ObterTodosAsync();
        // --- ASSERT ---
        var noContentResult = result.Result.Should().BeOfType<NoContentResult>().Subject;
        noContentResult.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task ObterTodos_DeveRetornarInternalServerError_QuandoOcorreExcecao()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterTodosAsync()).ThrowsAsync(new Exception("Erro de teste"));
        var controller = new ProdutoController(repoMock.Object);
        // --- ACT ---
        var result = await controller.ObterTodosAsync();
        // --- ASSERT ---
        var statusResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
        statusResult.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarOk_QuandoProdutoExistir()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        var produtoFake = new ProdutoTestsFixture().GerarListaValida(1).First();
        repoMock.Setup(r => r.ObterPorIdAsync(produtoFake.ProdutoID)).ReturnsAsync(produtoFake);
        var controller = new ProdutoController(repoMock.Object);
        // --- ACT ---
        var result = await controller.ObterPorId(produtoFake.ProdutoID);
        // --- ASSERT ---
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value.Should().BeAssignableTo<ProdutoBResponse>().Subject;
        response.ProdutoID.Should().Be(produtoFake.ProdutoID);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFound_QuandoProdutoNaoExistir()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Produto)null);
        var controller = new ProdutoController(repoMock.Object);
        // --- ACT ---
        var result = await controller.ObterPorId(Guid.NewGuid());
        // --- ASSERT ---
        var notFoundResult = result.Result.Should().BeOfType<NotFoundResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarInternalServerError_QuandoOcorreExcecao()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Erro de teste"));
        var controller = new ProdutoController(repoMock.Object);
        // --- ACT ---
        var result = await controller.ObterPorId(Guid.NewGuid());
        // --- ASSERT ---
        var statusResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
        statusResult.StatusCode.Should().Be(500);
    }
}