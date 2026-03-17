using Microsoft.EntityFrameworkCore;
using Prateleira_Universal.Data.context;
using Prateleira_Universal.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Prateleira_Universal.Testes.TesteDeIntegracao
{
    public class ProdutoIntegrationTests_DELETE
    {
        private DbContextOptions<AppDbContext> CriarOpcoesBanco()
        {
            // Cria um banco de dados novo na memória a cada teste para não poluir
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task DeletarProduto_DeveRemoverDoBancoReal()
        {
            // --- ARRANGE ---
            var options = CriarOpcoesBanco();
            using var context = new AppDbContext(options);
            var repo = new ProdutoRepository(context);
            var controller = new ProdutoController(repo);

            var produtoId = Guid.NewGuid();

            // Em vez de null, passamos um dicionário novo
            var produto = new Produto(
                produtoId,
                "Erva Premium",
                EnumCategoria.Erva_Mate,
                "Descrição da erva",
                25.0m,
                new Dictionary<string, string> { { "Marca", "X" } } // Especificações preenchidas
            );

            context.Produtos.Add(produto);
            await context.SaveChangesAsync();
            // --- ACT ---
            var result = await controller.DeletarProdutoAsync(produtoId);

            // --- ASSERT ---
            result.Should().BeOfType<NoContentResult>();

            // A prova de fogo: O banco de dados está vazio?
            var produtoNoBanco = await context.Produtos.FindAsync(produtoId);
            produtoNoBanco.Should().BeNull(); // Se for null, a integração funcionou!
        }

        [Fact]
        public async Task DeletarProduto_DeveRetornarNotFound_QuandoProdutoNaoExistir()
        {
            // --- ARRANGE ---
            var options = CriarOpcoesBanco();
            using var context = new AppDbContext(options);
            var repo = new ProdutoRepository(context);
            var controller = new ProdutoController(repo);
            var produtoId = Guid.NewGuid(); // ID que não existe no banco
            // --- ACT ---
            var result = await controller.DeletarProdutoAsync(produtoId);
            // --- ASSERT ---
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeletarProduto_DeveRetornarBadRequest_QuandoIdInvalido()
        {
            // --- ARRANGE ---
            var options = CriarOpcoesBanco();
            using var context = new AppDbContext(options);
            var repo = new ProdutoRepository(context);
            var controller = new ProdutoController(repo);
            var produtoId = Guid.Empty; // ID inválido
            // --- ACT ---
            var result = await controller.DeletarProdutoAsync(produtoId);
            // --- ASSERT ---
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}