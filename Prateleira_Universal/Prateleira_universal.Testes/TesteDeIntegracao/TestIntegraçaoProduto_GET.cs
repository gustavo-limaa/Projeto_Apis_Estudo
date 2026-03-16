using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prateleira_Universal.Controllers;
using Prateleira_Universal.Data.context;

namespace Prateleira_Universal.Testes.TesteDeIntegracao
{
    public class TestIntegraçaoProduto_GET : IntegrationTestBase
    {
        [Fact]
        public async Task ObterProduto_DeveRetornarProdutoExistente()
        {
            // --- ARRANGE ---
            using var context = CriarContexto();
            var repo = new ProdutoRepository(context);
            var controller = new ProdutoController(repo);
            var produtoId = Guid.NewGuid();
            var produto = new Produto(
                produtoId,
                "Erva Mate Premium",
                EnumCategoria.Erva_Mate,
                "Descrição da erva mate premium",
                30.0m,
                new Dictionary<string, string> { { "Marca", "Premium" } }
            );
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();
            // --- ACT ---
            var result = await controller.ObterPorId(produtoId);

            // --- ASSERT ---
            // 1. Primeiro, extraímos o OkObjectResult de dentro do ActionResult
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;

            // 2. Agora, verificamos se o valor dentro do Ok é o seu DTO de resposta
            var produtoRetornado = okResult.Value.Should().BeOfType<ProdutoBResponse>().Subject;

            // 3. Validações finais (usando os nomes das propriedades do seu DTO)
            produtoRetornado.Nome.Should().Be("Erva Mate Premium");
        }
    }
}