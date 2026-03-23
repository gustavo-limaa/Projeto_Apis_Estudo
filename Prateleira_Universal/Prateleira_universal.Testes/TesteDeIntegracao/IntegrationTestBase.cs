using Microsoft.EntityFrameworkCore;
using Prateleira_Universal.Domain.interfaces;
using Prateleira_Universal.interfaces.RefitApi;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Prateleira_Universal.Testes.TesteDeIntegracao;

[Collection("Produto Collection")] // Isso aqui conecta com a sua Fixture que limpa o banco
public class IntegrationTestBase
{
    protected readonly IApiProduto ApiClient;
    protected readonly ProdutoTestsFixture Fixture;

    public IntegrationTestBase(ProdutoTestsFixture fixture)
    {
        Fixture = fixture;

        // Criamos o HttpClient que "fala" com a API em memória (WebApplicationFactory)
        var httpClient = fixture.CreateClient();

        // O Refit assume o controle desse HttpClient
        ApiClient = RestService.For<IApiProduto>(httpClient);
        // No seu IntegrationTestBase.cs
        var settings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            })
        };

        // Use as settings aqui
        ApiClient = RestService.For<IApiProduto>(httpClient, settings);
    }
}