using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_universal.Testes.TesteUnitarios;

[Collection("Produto Collection")]
public class TesteProduto_Delete
{
    [Fact]
    public async Task DeletarProduto_DeveRetornarNoContent_QuandoProdutoForDeletadoComSucesso()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        var produtoId = Guid.NewGuid();
        // Configuramos o Mock para dizer que o produto existe e que a exclusão foi bem-sucedida
        repoMock.Setup(r => r.ObterPorIdAsync(produtoId)).ReturnsAsync(new Produto(
            produtoId,
            "Produto Teste",
            EnumCategoria.Erva_Mate,
            "Descrição do produto teste",
            50.0m,
            new Dictionary<string, string> { { "Sabor", "Menta" } }
        ));
        repoMock.Setup(r => r.DeletarAsync(It.IsAny<Produto>())).Returns(Task.CompletedTask);
        repoMock.Setup(r => r.SalvarAlteracoesAsync()).ReturnsAsync(true);
        var controller = new ProdutoController(repoMock.Object);
        // --- ACT ---
        var result = await controller.DeletarProdutoAsync(produtoId);
        // --- ASSERT ---
        var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
        noContentResult.StatusCode.Should().Be(204);
        // Verificamos se o Repositório foi chamado para obter, deletar e salvar
        repoMock.Verify(r => r.ObterPorIdAsync(produtoId), Times.Once);
        repoMock.Verify(r => r.DeletarAsync(It.IsAny<Produto>()), Times.Once);
        repoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeletarProduto_DeveRetornarNotFound_QuandoProdutoNaoExistir()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        var produtoId = Guid.NewGuid();
        // Configuramos o Mock para dizer que o produto não existe
        repoMock.Setup(r => r.ObterPorIdAsync(produtoId)).ReturnsAsync((Produto)null);
        var controller = new ProdutoController(repoMock.Object);
        // --- ACT ---
        var result = await controller.DeletarProdutoAsync(produtoId);
        // --- ASSERT ---
        var notFoundResult = result.Should().BeOfType<NotFoundResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
        // Verificamos se o Repositório foi chamado para obter, mas não para deletar ou salvar
        repoMock.Verify(r => r.ObterPorIdAsync(produtoId), Times.Once);
        repoMock.Verify(r => r.DeletarAsync(It.IsAny<Produto>()), Times.Never);
        repoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeletarProduto_DeveRetornarInternalServerError_QuandoOcorreExcecao()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        var produtoId = Guid.NewGuid();
        // Configuramos o Mock para lançar uma exceção ao tentar obter o produto
        repoMock.Setup(r => r.ObterPorIdAsync(produtoId)).ThrowsAsync(new Exception("Erro de teste"));
        var controller = new ProdutoController(repoMock.Object);
        // --- ACT ---
        var result = await controller.DeletarProdutoAsync(produtoId);
        // --- ASSERT ---
        var statusResult = result.Should().BeOfType<ObjectResult>().Subject;
        statusResult.StatusCode.Should().Be(500);
        // Verificamos se o Repositório foi chamado para obter, mas não para deletar ou salvar
        repoMock.Verify(r => r.ObterPorIdAsync(produtoId), Times.Once);
        repoMock.Verify(r => r.DeletarAsync(It.IsAny<Produto>()), Times.Never);
        repoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeletarProduto_DeveRetornarBadRequest_QuandoProdutoIdForInvalido()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        var controller = new ProdutoController(repoMock.Object);
        // --- ACT ---
        var result = await controller.DeletarProdutoAsync(Guid.Empty);
        // --- ASSERT ---
        var badRequestResult = result.Should().BeOfType<BadRequestResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
        // Verificamos se o Repositório não foi chamado para obter, deletar ou salvar
        repoMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Never);
        repoMock.Verify(r => r.DeletarAsync(It.IsAny<Produto>()), Times.Never);
        repoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeletarProduto_DeveRetornarBadRequest_QuandoSalvarAlteracoesFalhar()
    {
        // --- ARRANGE ---
        var repoMock = new Mock<IProdutoRepository>();
        var produtoId = Guid.NewGuid();
        // Configuramos o Mock para dizer que o produto existe e que a exclusão foi bem-sucedida, mas o salvamento falhou
        repoMock.Setup(r => r.ObterPorIdAsync(produtoId)).ReturnsAsync(new Produto(
            produtoId,
            "Produto Teste",
            EnumCategoria.Erva_Mate,
            "Descrição do produto teste",
            50.0m,
            new Dictionary<string, string> { { "Sabor", "Menta" } }
        ));
        repoMock.Setup(r => r.DeletarAsync(It.IsAny<Produto>())).Returns(Task.CompletedTask);
        repoMock.Setup(r => r.SalvarAlteracoesAsync()).ReturnsAsync(false);
        var controller = new ProdutoController(repoMock.Object);
        // --- ACT ---
        var result = await controller.DeletarProdutoAsync(produtoId);
        // --- ASSERT ---
        var badRequestResult = result.Should().BeOfType<BadRequestResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
        // Verificamos se o Repositório foi chamado para obter, deletar e salvar
        repoMock.Verify(r => r.ObterPorIdAsync(produtoId), Times.Once);
        repoMock.Verify(r => r.DeletarAsync(It.IsAny<Produto>()), Times.Once);
        repoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }
}