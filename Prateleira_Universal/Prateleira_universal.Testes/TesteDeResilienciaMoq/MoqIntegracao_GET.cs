using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_universal.Testes.TesteDeResilienciaMoq
{
    public class MoqIntegracao_GET : IClassFixture<MockProdutoFactory>
    {
        private readonly MockProdutoFactory _factory;
        private readonly HttpClient _client;

        public MoqIntegracao_GET(MockProdutoFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task ObterTodos_QuandoBancoExplodir_DeveRetornar500()
        {
            // Arrange - Aqui o Moq faz a mágica!
            _factory.RepoMock
                .Setup(r => r.ObterTodosAsync())
                .ThrowsAsync(new Exception("Pane no sistema, alguém desconfigurou!"));

            // Act
            var response = await _client.GetAsync("/api/produtos");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task ObterPorId_QuandoBancoExplodir_DeveRetornar500()
        {
            // Arrange - Aqui o Moq faz a mágica!
            _factory.RepoMock
                .Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Pane no sistema, alguém desconfigurou!"));
            var idTeste = Guid.NewGuid();
            // Act
            var response = await _client.GetAsync($"/api/produtos/{idTeste}");
            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task ObterPorId_QuandoProdutoNaoExistir_DeveRetornar404()
        {
            // Arrange - Aqui o Moq faz a mágica!
            _factory.RepoMock
                .Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Produto)null); // Simula produto não encontrado
            var idTeste = Guid.NewGuid();
            // Act
            var response = await _client.GetAsync($"/api/produtos/{idTeste}");
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ObterPorId_QuandoProdutoExistir_DeveRetornar200()
        {
            // Arrange - Aqui o Moq faz a mágica!
            var produtoFake = new MockProdutoFactory()._produtoFaker.Generate();
            _factory.RepoMock
                .Setup(r => r.ObterPorIdAsync(produtoFake.ProdutoID))
                .ReturnsAsync(produtoFake); // Simula produto encontrado
            // Act
            var response = await _client.GetAsync($"/api/produtos/{produtoFake.ProdutoID}");
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ObterTodos_QuandoProdutosExistirem_DeveRetornar200()
        {
            // Arrange - Aqui o Moq faz a mágica!
            var produtosFake = new MockProdutoFactory().GerarListaValida(5);
            _factory.RepoMock
                .Setup(r => r.ObterTodosAsync())
                .ReturnsAsync(produtosFake); // Simula produtos encontrados
            // Act
            var response = await _client.GetAsync("/api/produtos");
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}