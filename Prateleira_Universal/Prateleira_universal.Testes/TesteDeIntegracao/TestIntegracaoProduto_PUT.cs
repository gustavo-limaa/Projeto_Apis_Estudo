using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_Universal.Testes.TesteDeIntegracao
{
    public class TestIntegracaoProduto_PUT : IntegrationTestBase
    {
        [Fact]
        public async Task AtualizarProduto_DevePersistirAlteracoesNoBancoReal()
        {
            // --- ARRANGE ---
            // Pega o contexto pronto da base, já configurado com InMemory
            using var context = CriarContexto();

            var repo = new ProdutoRepository(context);
            var controller = new ProdutoController(repo);

            var produtoId = Guid.NewGuid();
            var produtoOriginal = new Produto(
                produtoId,
                "Erva Velha",
                EnumCategoria.Erva_Mate,
                "Desc",
                10m,
                new Dictionary<string, string>()
            );

            context.Produtos.Add(produtoOriginal);
            await context.SaveChangesAsync();

            // Essencial para o teste de integração: força o EF a esquecer o objeto
            // que está na memória e buscar a versão "gravada" no próximo passo.
            context.ChangeTracker.Clear();

            var requestUpdate = new ProdutoBUpdate(
                "Erva Nova",
                EnumCategoria.Erva_Mate,
                15.0m,
                "Nova Descrição",
                new Dictionary<string, string> { { "Sabor", "Menta" } }
            );

            // --- ACT ---
            await controller.AtualizarProdutoAsync(produtoId, requestUpdate);

            // --- ASSERT ---
            var produtoNoBanco = await context.Produtos.FindAsync(produtoId);

            produtoNoBanco.Should().NotBeNull();
            produtoNoBanco.Nome.Should().Be("Erva Nova");
            produtoNoBanco.Preco.Should().Be(15.0m);
            produtoNoBanco.Especificacoes.Should().ContainKey("Sabor").WhoseValue.Should().Be("Menta");
        }
    }
}