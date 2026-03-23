using Microsoft.AspNetCore.Mvc;
using Prateleira_universal.Testes.ContextTest;
using Prateleira_Universal.UseCases.Dtos.ProdutoBaseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_Universal.Testes.TesteDeIntegracao;

[Collection("Produto Collection")]
public class TestIntegracaoProduto_POST
{
    private readonly ProdutoTestsFixture _fixture;
    private readonly HttpClient _client;

    public TestIntegracaoProduto_POST(ProdutoTestsFixture fixture)
    {
        _fixture = fixture;
        _client = _fixture.CreateClient();
    }

    [Fact]
    public async Task CriarProduto_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var request = _fixture.GerarRequestValido(); // Olha a limpeza aqui!

        // Act
        var response = await _client.PostAsJsonAsync("/api/produtos", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Testar_CriarProduto_Retornar_BadRequest()
    {
        // Arrange
        ProdutoBRequest request = null; // Simula um corpo de requisição nulo
        // Act
        var response = await _client.PostAsJsonAsync("/api/produtos", request);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Testar_CriarProduto_Retornar_BADREQUEST()
    {
        // Arrange
        var request = new ProdutoBRequest(
            "Produto com Nome Muito Longo que Excede o Limite de 50 Caracteres para Testar o Erro",
            "Descrição do produto",
            EnumCategoria.Erva_Mate,
            new Dictionary<string, string> { { "Chave", "Valor" } },
            99.99m
        );
        // Act
        var response = await _client.PostAsJsonAsync("/api/produtos", request);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Testar_CriarProduto_Retornar_BADREQUEST_PorPrecoInvalido()
    {
        // Arrange
        var request = new ProdutoBRequest(
            "Produto Válido",
            "Descrição do produto",
            EnumCategoria.Erva_Mate,
            new Dictionary<string, string> { { "Chave", "Valor" } },
            -10.00m // Preço inválido (negativo)
        );
        // Act
        var response = await _client.PostAsJsonAsync("/api/produtos", request);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CriarProduto_ComDadosValidos_DeveRetornarProdutoCriado()
    {
        // Arrange
        var request = _fixture.GerarRequestValido();
        // Act
        var response = await _client.PostAsJsonAsync("/api/produtos", request);
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var produtoCriado = await response.Content.ReadFromJsonAsync<ProdutoBResponse>();
        Assert.NotNull(produtoCriado);
        Assert.Equal(request.Nome, produtoCriado.Nome);
        Assert.Equal(request.Descricao, produtoCriado.Descricao);
        Assert.Equal(request.Tipo, produtoCriado.Tipo);
        Assert.Equal(request.Preco, produtoCriado.Preco);
        Assert.Equal(request.Especificacoes, produtoCriado.Especificacoes);
    }
}