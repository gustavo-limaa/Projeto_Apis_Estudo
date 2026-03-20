using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_universal.Testes.TesteDeResilienciaMoq
{
    public class MoqIntegracao_post : IClassFixture<MockProdutoFactory>
    {
        private readonly MockProdutoFactory _factory;
        private readonly HttpClient _client;

        public MoqIntegracao_post(MockProdutoFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CriarProduto_QuandoBancoExplodir_DeveRetornar500()
        {
            // Arrange - Aqui o Moq faz a mágica!
            _factory.RepoMock
                .Setup(r => r.AdicionarAsync(It.IsAny<Produto>()))
                .ThrowsAsync(new Exception("Pane no sistema, alguém desconfigurou!"));

            var novoProduto = _factory._produtoFaker.Generate();

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(novoProduto),
                Encoding.UTF8,
                "application/json");
            // Act
            var response = await _client.PostAsync("/api/produtos", content);
            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}