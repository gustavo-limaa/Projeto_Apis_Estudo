using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Prateleira_Universal.Testes.TesteDeIntegracao;

public class TestIntegracaoProduto_POST : IntegrationTestBase
{
    [Fact]
    public async Task CriarProduto_DeveAdicionarProdutoNoBancoReal()
    {
        // --- ARRANGE ---
        using var context = CriarContexto();
        var repo = new ProdutoRepository(context);
        var controller = new ProdutoController(repo);

        // --- ARRANGE ---
        var request = new ProdutoBRequest(
            Nome: "Erva Nova",
            Descricao: "Nova Descrição",
            Tipo: EnumCategoria.Erva_Mate,
            Especificacoes: new Dictionary<string, string> { { "Sabor", "Menta" } },
            Preco: 15.0m
        );

        var actionResult = await controller.CriarProdutoAsync(request);

        // --- ASSERT ---
        // Compatibiliza cenários onde o controller retorna CreatedAtActionResult diretamente
        // ou retorna ActionResult<T> que expõe a propriedade "Result".
        CreatedAtActionResult createdResult = null;

        if (actionResult is CreatedAtActionResult ca)
        {
            createdResult = ca;
        }
        else
        {
            // tenta pegar a propriedade "Result" por reflexão (quando o tipo é ActionResult<T>)
            var resultProp = actionResult?.GetType().GetProperty("Result", BindingFlags.Public | BindingFlags.Instance);
            if (resultProp != null)
            {
                var resultValue = resultProp.GetValue(actionResult);
                createdResult = resultValue as CreatedAtActionResult;
            }
        }

        createdResult.Should().NotBeNull();
        var produtoCriado = createdResult.Value.Should().BeOfType<ProdutoBResponse>().Subject;

        produtoCriado.Nome.Should().Be("Erva Nova");

        // Verificação final: o produto realmente foi gravado na memória?
        // Ajuste o nome da propriedade de ID aqui conforme o seu DTO
        var idParaBusca = produtoCriado.ProdutoID;

        var produtoNoBanco = await context.Produtos.FindAsync(idParaBusca);
        produtoNoBanco.Should().NotBeNull();
        produtoNoBanco.Nome.Should().Be("Erva Nova");
    }
}