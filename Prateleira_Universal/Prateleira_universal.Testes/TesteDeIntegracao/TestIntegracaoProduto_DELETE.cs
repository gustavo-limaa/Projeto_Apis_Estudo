using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Prateleira_universal.Testes.ContextTest;
using Prateleira_Universal.Data.context;
using Prateleira_Universal.Repositorios;
using Prateleira_Universal.UseCases.Dtos.ProdutoBaseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prateleira_Universal.Testes.TesteDeIntegracao
{
    [Collection("Produto Collection")] // 1. Usa a coleção que limpa o banco
    public class ProdutoIntegrationTests_DELETE
    {
        private readonly ProdutoTestsFixture _fixture;
        private readonly HttpClient _client;

        public ProdutoIntegrationTests_DELETE(ProdutoTestsFixture fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient(); // 2. O client fala com a API real
        }

        [Fact]
        public async Task DeletarProduto_DeveRemoverDoBancoReal()
        {
            // --- ARRANGE ---
            // Usamos o Bogus que você criou na Fixture!
            var produtoParaCriar = _fixture.GerarRequestValido();
            // --- ARRANGE (Final do Setup) ---
            var postResponse = await _client.PostAsJsonAsync("api/produtos", produtoParaCriar);
            postResponse.EnsureSuccessStatusCode();

            var produtoCriado = await postResponse.Content.ReadFromJsonAsync<ProdutoBResponse>();

            // O "Check" de elite:
            produtoCriado.Should().NotBeNull("O produto precisa ser criado no banco antes de testar o DELETE");
            produtoCriado.ProdutoID.Should().NotBe(Guid.Empty);

            var idParaDeletar = produtoCriado.ProdutoID;

            // --- ACT ---
            var result = await _client.DeleteAsync($"api/produtos/{idParaDeletar}");
            // --- ASSERT ---
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // A prova de fogo:
            // Se o produto foi deletado, o GET não deve encontrá-lo (404)
            var getResponse = await _client.GetAsync($"/api/produtos/{idParaDeletar}");

            // AJUSTE AQUI:
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeletarProduto_Inexistente_DeveRetornarNotFound()
        {
            // --- ARRANGE ---
            var idInexistente = Guid.NewGuid();
            // --- ACT ---
            var result = await _client.DeleteAsync($"api/produtos/{idInexistente}");
            // --- ASSERT ---
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeletarProduto_ComErroInterno_DeveRetornarInternalServerError()
        {
            // --- ARRANGE ---
            // Para simular um erro interno, podemos usar um ID que sabemos que causará um erro (ex: um ID malformado)
            var idInvalido = 123123124;
            // --- ACT ---
            var result = await _client.DeleteAsync($"api/produtos/{idInvalido}");
            // --- ASSERT ---
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeletarProduto_ID_Inexistente_DeveRetornarNotFound()
        {
            // --- ARRANGE ---
            // Geramos um Guid válido (formato correto), mas que NUNCA foi criado no banco
            var idQueNaoExiste = Guid.NewGuid();

            // --- ACT ---
            var result = await _client.DeleteAsync($"api/produtos/{idQueNaoExiste}");

            // --- ASSERT ---
            // Aqui testamos se o seu "if (produto is null)" está acordado!
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeletarProduto_Valido_DeveRetornarNoContent()
        {
            // --- ARRANGE ---
            var produtoParaCriar = _fixture.GerarRequestValido();
            var postResponse = await _client.PostAsJsonAsync("api/produtos", produtoParaCriar);
            postResponse.EnsureSuccessStatusCode();
            var produtoCriado = await postResponse.Content.ReadFromJsonAsync<ProdutoBResponse>();
            produtoCriado.Should().NotBeNull();
            produtoCriado.ProdutoID.Should().NotBe(Guid.Empty);
            var idParaDeletar = produtoCriado.ProdutoID;
            // --- ACT ---
            var result = await _client.DeleteAsync($"api/produtos/{idParaDeletar}");
            // --- ASSERT ---
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}