using Microsoft.AspNetCore.Mvc;
using Prateleira_universal.Testes.ContextTest;
using Prateleira_Universal.Controllers;
using Prateleira_Universal.Data.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_Universal.Testes.TesteDeIntegracao
{
    [Collection("Produto Collection")]
    public class TestIntegraçaoProduto_GET
    {
        private readonly ProdutoTestsFixture _fixture;
        private readonly HttpClient _client;

        public TestIntegraçaoProduto_GET(ProdutoTestsFixture fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();

            _fixture.Resetarbanco(); // Limpa o banco antes de cada teste para garantir isolamento
        }

        [Fact]
        public async Task ObterProduto_PorId_DeveRetornarProduto()
        {
            // Arrange
            var request = _fixture.GerarRequestValido();
            var postResponse = await _client.PostAsJsonAsync("/api/produtos", request);
            postResponse.EnsureSuccessStatusCode();
            var produtoCriado = await postResponse.Content.ReadFromJsonAsync<ProdutoBResponse>();
            // Act
            var getResponse = await _client.GetAsync($"/api/produtos/{produtoCriado.ProdutoID}");
            getResponse.EnsureSuccessStatusCode();
            var produtoObtido = await getResponse.Content.ReadFromJsonAsync<ProdutoBResponse>();
            // Assert
            Assert.NotNull(produtoObtido);
            Assert.Equal(produtoCriado.ProdutoID, produtoObtido.ProdutoID);
            Assert.Equal(produtoCriado.Nome, produtoObtido.Nome);
            Assert.Equal(produtoCriado.Descricao, produtoObtido.Descricao);
            Assert.Equal(produtoCriado.Tipo, produtoObtido.Tipo);
            Assert.Equal(produtoCriado.Preco, produtoObtido.Preco);
            Assert.Equal(produtoCriado.Especificacoes, produtoObtido.Especificacoes);
        }

        [Fact]
        public async Task ObterProduto_PorId_Inexistente_DeveRetornarNotFound()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();
            // Act
            var getResponse = await _client.GetAsync($"/api/produtos/{idInexistente}");
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task ObterTodosProdutos_DeveRetornarLista()
        {
            // Arrange
            var request1 = _fixture.GerarRequestValido();
            var request2 = _fixture.GerarRequestValido();
            await _client.PostAsJsonAsync("/api/produtos", request1);
            await _client.PostAsJsonAsync("/api/produtos", request2);
            // Act
            var getResponse = await _client.GetAsync("/api/produtos");
            getResponse.EnsureSuccessStatusCode();
            var produtosObtidos = await getResponse.Content.ReadFromJsonAsync<List<ProdutoBResponse>>();
            // Assert
            Assert.NotNull(produtosObtidos);
            Assert.True(produtosObtidos.Count >= 2, "Deve retornar pelo menos os produtos criados");
        }

        [Fact]
        public async Task ObterTodosProdutos_QuandoNaoExistirem_DeveRetornarListaVazia()
        {
            // Arrange
            // (A coleção limpa o banco antes de cada teste, então não precisamos criar produtos aqui)
            //
            var getResponse = await _client.GetAsync("/api/produtos");
            getResponse.EnsureSuccessStatusCode();
            var produtosObtidos = await getResponse.Content.ReadFromJsonAsync<List<ProdutoBResponse>>();
            // Assert
            Assert.NotNull(produtosObtidos);
            Assert.Empty(produtosObtidos);
        }

        [Fact]
        public async Task ObterProduto_PorId_ComIdInvalido_DeveRetornarBadRequest()
        {
            // Arrange
            var idInvalido = "abc"; // ID que não é um GUID válido
            // Act
            var getResponse = await _client.GetAsync($"/api/produtos/{idInvalido}");
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, getResponse.StatusCode);
        }
    }
}