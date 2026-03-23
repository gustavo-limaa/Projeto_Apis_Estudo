using Prateleira_Universal.Domain.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_universal.Testes.TesteUnitarios
{
    [Collection("Produto Collection")]
    public class TesteProduto_PUT
    {
        [Fact]
        public async Task AtualizarProduto_DeveRetornarOk_QuandoProdutoForAtualizadoComSucesso()
        {
            // --- ARRANGE ---
            var repoMock = new Mock<IProdutoRepository>();
            var produtoId = Guid.NewGuid();
            var produtoExistente = new Produto(
                produtoId,
                "Produto Existente",
                EnumCategoria.Erva_Mate,
                "Descrição do produto existente",
                50.0m,
                new Dictionary<string, string> { { "Sabor", "Menta" } }
            ); var requestFake = new ProdutoBUpdate(
            "Produto Atualizado",                                 // 1. Nome (string)
            EnumCategoria.Erva_Mate,                              // 2. Tipo (Enum) - MUDOU A ORDEM
             60.0m,                                                // 3. Preco (decimal) - MUDOU A ORDEM
            "Descrição do produto atualizado",                   // 4. Descricao (string)
             new Dictionary<string, string> { { "Sabor", "Limão" } } // 5. Especificacoes (Dict)
            );
            // Configuramos o Mock para dizer que o produto existe e que a atualização foi bem-sucedida
            repoMock.Setup(r => r.ObterPorIdAsync(produtoId)).ReturnsAsync(produtoExistente);
            repoMock.Setup(r => r.AtualizarAsync(It.IsAny<Produto>())).Returns(Task.CompletedTask);
            repoMock.Setup(r => r.SalvarAlteracoesAsync()).ReturnsAsync(true);
            var controller = new ProdutoController(repoMock.Object);
            // --- ACT ---
            var result = await controller.AtualizarProdutoAsync(produtoId, requestFake);
            // --- ASSERT ---
            var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
            // Remova as validações de response.Nome, pois não há corpo no 204.
            noContentResult.StatusCode.Should().Be(204);
            // Verificamos se o Repositório foi chamado para obter, atualizar e salvar
            repoMock.Verify(r => r.ObterPorIdAsync(produtoId), Times.Once);
            repoMock.Verify(r => r.ObterPorIdAsync(produtoId), Times.Once);
            repoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
        }

        [Fact]
        public async Task AtualizarProduto_DeveRetornarNotFound_QuandoProdutoNaoExistir()
        {
            // --- ARRANGE ---
            var repoMock = new Mock<IProdutoRepository>();
            var produtoId = Guid.NewGuid();
            var requestFake = new ProdutoBUpdate(
                "Produto Atualizado",                                 // 1. Nome (string)
                EnumCategoria.Erva_Mate,                              // 2. Tipo (Enum) - MUDOU A ORDEM
                 60.0m,                                                // 3. Preco (decimal) - MUDOU A ORDEM
                "Descrição do produto atualizado",                   // 4. Descricao (string)
                 new Dictionary<string, string> { { "Sabor", "Limão" } } // 5. Especificacoes (Dict)
                );
            // Configuramos o Mock para dizer que o produto não existe
            repoMock.Setup(r => r.ObterPorIdAsync(produtoId)).ReturnsAsync((Produto)null);
            var controller = new ProdutoController(repoMock.Object);
            // --- ACT ---
            var result = await controller.AtualizarProdutoAsync(produtoId, requestFake);
            // --- ASSERT ---
            var notFoundResult = result.Should().BeOfType<NotFoundResult>().Subject;
            notFoundResult.StatusCode.Should().Be(404);
            // Verificamos se o Repositório foi chamado para obter, mas não para atualizar ou salvar
            repoMock.Verify(r => r.ObterPorIdAsync(produtoId), Times.Once);
            repoMock.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Never);
            repoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
        }

        [Fact]
        public async Task AtualizarProduto_DeveRetornarInternalServerError_QuandoOcorreExcecao()
        {
            // --- ARRANGE ---
            var repoMock = new Mock<IProdutoRepository>();
            var produtoId = Guid.NewGuid();
            var requestFake = new ProdutoBUpdate(
                "Produto Atualizado",                                 // 1. Nome (string)
                EnumCategoria.Erva_Mate,                              // 2. Tipo (Enum) - MUDOU A ORDEM
                 60.0m,                                                // 3. Preco (decimal) - MUDOU A ORDEM
                "Descrição do produto atualizado",                   // 4. Descricao (string)
                 new Dictionary<string, string> { { "Sabor", "Limão" } } // 5. Especificacoes (Dict)
                );
            // Configuramos o Mock para lançar uma exceção ao tentar obter o produto
            repoMock.Setup(r => r.ObterPorIdAsync(produtoId)).ThrowsAsync(new Exception("Erro de teste"));
            var controller = new ProdutoController(repoMock.Object);
            // --- ACT ---
            var result = await controller.AtualizarProdutoAsync(produtoId, requestFake);
            // --- ASSERT ---
            var statusResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusResult.StatusCode.Should().Be(500);
            // Verificamos se o Repositório foi chamado para obter, mas não para atualizar ou salvar
            repoMock.Verify(r => r.ObterPorIdAsync(produtoId), Times.Once);
            repoMock.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Never);
            repoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
        }

        [Fact]
        public async Task AtualizarProduto_DeveRetornarBadRequest_QuandoRequestForNulo()
        {
            // --- ARRANGE ---
            var repoMock = new Mock<IProdutoRepository>();
            var produtoId = Guid.NewGuid();
            var controller = new ProdutoController(repoMock.Object);
            // --- ACT ---
            var result = await controller.AtualizarProdutoAsync(produtoId, null);
            // --- ASSERT ---
            var badRequestResult = result.Should().BeOfType<BadRequestResult>().Subject;
            badRequestResult.StatusCode.Should().Be(400);
            // Verificamos se o Repositório não foi chamado para obter, atualizar ou salvar
            repoMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Never);
            repoMock.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Never);
            repoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
        }
    }
}