using Prateleira_Universal.Testes.TesteDeIntegracao;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_universal.Testes.TesteDeIntegracao;

public class RefitTestIntegracao : IntegrationTestBase
{
    public RefitTestIntegracao(ProdutoTestsFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Deve_Criar_E_Recuperar_Produto_Usando_Refit()
    {
        // 1. Arrange: Dados na mão, sem Bogus, sem Dicionário complexo
        var requestManual = new ProdutoBRequest(
            "Teclado Gamer",
            "Teclado mecanico rgb",
            EnumCategoria.Erva_Mate,
            new Dictionary<string, string>(), // Vazio para testar
            150.00m
        );

        try
        {
            // 2. Act
            var resposta = await ApiClient.CriarProduto(requestManual);
            Assert.NotNull(resposta);
        }
        catch (ValidationApiException ex)
        {
            // Esse é o SEGREDO: Inspecione o 'ex.Content' aqui no Debug
            var corpoDoErro = ex.Content;
            throw; // Coloque um Breakpoint aqui!
        }
    }

    [Fact]
    public async Task Deve_criar_e_Deletar_Produto_Refit()
    {
        var novoProduto = Fixture.GerarRequestValido();

        var request = await ApiClient.CriarProduto(novoProduto);

        var response = await ApiClient.DeletarProdutoPorID(request.ProdutoID);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Deve_Cria_E_Att_produto()
    {
        var novoproduto = Fixture.GerarRequestValido();
        var request = await ApiClient.CriarProduto(novoproduto);

        var DadosParaAtt = new ProdutoBUpdate
          (
            "menta",
            request.Tipo,
            55.99m,
            request.Descricao,
            request.Especificacoes
          );

        await ApiClient.AtualizarProdutoPorID(request.ProdutoID, DadosParaAtt);

        var buscar = await ApiClient.ObterTodosPorID(request.ProdutoID);

        Assert.NotNull(buscar);
        Assert.Equal("menta", buscar.Nome);
    }

    [Fact]
    public async Task Deve_Retornar_NotFound_Ao_Atualizar_Produto_Inexistente()
    {
        // 1. Arrange: Um ID que com certeza não está no MySQL
        var idInexistente = Guid.NewGuid();
        var dadosUpdate = new ProdutoBUpdate(
            "Teste", EnumCategoria.Erva_Mate, 10.0m, "Desc", new Dictionary<string, string>()
        );

        // 2. Act & Assert: "Eu espero que isso jogue uma ApiException"
        var excecao = await Assert.ThrowsAsync<ValidationApiException>(() =>
            ApiClient.AtualizarProdutoPorID(idInexistente, dadosUpdate)
        );

        // 3. Verificação Final: O status foi mesmo 404?
        Assert.Equal(HttpStatusCode.NotFound, excecao.StatusCode);
    }

    [Fact]
    public async Task Deve_Retornar_BadRequest_Ao_Atualizar_Com_Id_Vazio()
    {
        // 1. Arrange: O ID que o .NET cria quando não damos valor: Guid.Empty
        var idVazio = Guid.Empty;
        var dados = Fixture.GerarRequestValido(); // Pode usar o request normal

        // Converte o request para o DTO de Update (ou cria um novo)
        var dadosUpdate = new ProdutoBUpdate(dados.Nome, dados.Tipo, dados.Preco, dados.Descricao, dados.Especificacoes);

        var excecao = await Record.ExceptionAsync(() =>
     ApiClient.AtualizarProdutoPorID(idVazio, dadosUpdate)
     );

        // Aí você verifica se ela é do tipo que você quer
        Assert.IsAssignableFrom<ApiException>(excecao);
        var apiEx = (ApiException)excecao;
        Assert.Equal(HttpStatusCode.BadRequest, apiEx.StatusCode);
    }

    [Fact]
    public async Task Deve_Cria_E_Att_produto_Com_Helpers()
    {
        // 1. Arrange (Usando Helper)
        var request = await ApiClient.CriarProduto(Fixture.GerarRequestValido());

        // 2. Act (Usando Helper para transformar o Request em Update)
        var dadosAtt = Fixture.GerarUpdateDe(request, "Menta Premium");
        await ApiClient.AtualizarProdutoPorID(request.ProdutoID, dadosAtt);

        // 3. Assert
        var buscar = await ApiClient.ObterTodosPorID(request.ProdutoID);
        Assert.Equal("Menta Premium", buscar.Nome);
    }

    [Fact]
    public async Task Deve_cria_e_dar_erro_com_helper()
    {
        var dadosDefeitoso = Fixture.GerarRequestComErro("preco-negativo");

        var excecao = await Record.ExceptionAsync(() => ApiClient.CriarProduto(dadosDefeitoso));

        Assert.NotNull(excecao);
        Assert.IsAssignableFrom<ApiException>(excecao);

        var apiEx = (ApiException)excecao;
        Assert.Equal(HttpStatusCode.BadRequest, apiEx.StatusCode);
    }
}