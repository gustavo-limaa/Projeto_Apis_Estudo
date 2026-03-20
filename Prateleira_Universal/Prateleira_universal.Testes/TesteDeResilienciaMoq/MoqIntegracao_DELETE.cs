using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Prateleira_Universal.Modelos;

namespace Prateleira_universal.Testes.TesteDeResilienciaMoq
{
    public class MoqIntegracao_DELETE : IClassFixture<MockProdutoFactory>
    {
        private readonly MockProdutoFactory _factory;
        private readonly HttpClient _client;

        public MoqIntegracao_DELETE(MockProdutoFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task DeletarProduto_QuandoBancoExplodir_DeveRetornar500()
        {
            // 1. Arrange
            var idTeste = Guid.NewGuid();
            var produtoFake = _factory.GerarListaValida(1).First();

            // Degrau 1: A Controller pede o produto para ver se ele existe
            _factory.RepoMock
                .Setup(r => r.ObterPorIdAsync(idTeste))
                .ReturnsAsync(produtoFake); // "Sim, o produto existe, pode prosseguir!"

            // Degrau 2: A Controller tenta deletar e aqui a gente BOOM! 💥
            _factory.RepoMock
                .Setup(r => r.DeletarAsync(It.IsAny<Prateleira_Universal.Modelos.Produto>()))
                .ThrowsAsync(new Exception("Pane no sistema!"));

            // 2. Act
            var response = await _client.DeleteAsync($"/api/produtos/{idTeste}");

            // 3. Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}