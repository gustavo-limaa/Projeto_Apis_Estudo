using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MySqlConnector;
using Moq;
using Prateleira_Universal.Modelos.Enums;
using Xunit;
using Bogus.Extensions;
using Prateleira_universal.Testes.ContextTest;

namespace Prateleira_universal.Testes.ContextTest;

// 1. ESTA É A FIXTURE (A Fábrica)
public class ProdutoTestsFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private string _connectionString;
    public Mock<IProdutoRepository> RepoMock { get; private set; }

    public ProdutoTestsFixture()
    {
        RepoMock = new Mock<IProdutoRepository>();
    }

    public List<Produto> GerarListaValida(int quantidade)
    {
        var faker = new Faker<Produto>()
            .CustomInstantiator(f => new Produto(
                Guid.NewGuid(),
                f.Commerce.ProductName().ClampLength(max: 50),
                f.PickRandom<EnumCategoria>(),
                f.Commerce.ProductDescription().ClampLength(max: 50),
                f.Finance.Amount(10, 100),
                new Dictionary<string, string> { { "Sabor", "Menta" } }
            ));

        return faker.Generate(quantidade);
    }

    public ProdutoBRequest GerarRequestValido()
    {
        var f = new Faker();
        return new ProdutoBRequest(
            f.Commerce.ProductName().ClampLength(max: 50),                 // 1. Nome (string)
            f.Commerce.ProductDescription().ClampLength(max: 50),          // 2. Descricao (string)
            EnumCategoria.Erva_Mate,                  // 3. Tipo (Enum)
            new Dictionary<string, string> { { "Origem", "MS" } }, // 4. Especificacoes (Dict)
            f.Finance.Amount(10, 100)                 // 5. Preco (decimal)
        );
    }

    public async Task InitializeAsync()
    {
        using (var scope = Services.CreateScope())
        {
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        await ClearDataBaseAsnyc();
    }

    public async Task DisposeAsync()
    {
        await Task.CompletedTask; // Não há necessidade de limpar o banco aqui, pois já fazemos isso no InitializeAsync antes de cada teste.
    }

    private async Task ClearDataBaseAsnyc()
    {
        using var Connectionstring = new MySqlConnection(_connectionString);

        await Connectionstring.OpenAsync();

        var sql =
            @"SET FOREIGN_KEY_CHECKS = 0;
            TRUNCATE TABLE Produtos;
            SET FOREIGN_KEY_CHECKS = 1;";

        using var command = new MySqlCommand(sql, Connectionstring);
        await command.ExecuteNonQueryAsync();
    }

    public void Resetarbanco()
    {
        ClearDataBaseAsnyc().GetAwaiter().GetResult();
    }
}

// 2. ESTA É A DEFINIÇÃO DA COLEÇÃO (A Etiqueta)
[CollectionDefinition("Produto Collection")]
public class ProdutoFixCollection : ICollectionFixture<ProdutoTestsFixture>
{
    // Não precisa de código aqui dentro!
}