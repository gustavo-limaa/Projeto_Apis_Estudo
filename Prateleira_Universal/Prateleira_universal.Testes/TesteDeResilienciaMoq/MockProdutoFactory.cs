using Bogus;
using Bogus.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_universal.Testes.TesteDeResilienciaMoq
{
    public class MockProdutoFactory : WebApplicationFactory<Program>
    {
        public Mock<IProdutoRepository> RepoMock { get; } = new();

        public readonly Faker<Produto> _produtoFaker;

        public MockProdutoFactory()
        {
            _produtoFaker = new Faker<Produto>()
                .CustomInstantiator(f =>
                {
                    // Gera de 1 a 3 especificações aleatórias
                    var especificacoesRando = new Dictionary<string, string>();
                    var qtdSpecs = f.Random.Int(1, 3);

                    for (int i = 0; i < qtdSpecs; i++)
                    {
                        // Usa palavras aleatórias do Bogus para Chave e Valor
                        var chave = f.Commerce.ProductAdjective() + i; // i evita chaves duplicadas
                        var valor = f.Commerce.Color();
                        especificacoesRando.TryAdd(chave, valor);
                    }

                    return new Produto(
                        Guid.NewGuid(),
                        f.Commerce.ProductName().ClampLength(max: 50),
                        f.PickRandom<EnumCategoria>(),
                        f.Commerce.ProductDescription().ClampLength(max: 50),
                        f.Finance.Amount(10, 100),
                        especificacoesRando // Agora é aleatório!
                    );
                });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // 1. Achamos o registro do Repositório Real
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IProdutoRepository));

                // 2. Arrancamos o motor real fora
                if (descriptor != null) services.Remove(descriptor);

                // 3. Colocamos o Mock no lugar!
                services.AddScoped<IProdutoRepository>(_ => RepoMock.Object);
            });
        }

        public List<Produto> GerarListaValida(int quantidade) => _produtoFaker.Generate(quantidade);

        public ProdutoBRequest GerarRequestValido()
        {
            var p = _produtoFaker.Generate(); // Gera uma entidade temporária
                                              // E transforma no Request que a API espera
            return new ProdutoBRequest(p.Nome, p.Descricao, p.Tipo, p.Especificacoes, p.Preco);
        }

        /// Na sua Factory
        public Produto GerarProduto(Action<Produto>? customizacao = null)
        {
            var p = _produtoFaker.Generate();

            // Se você quer "estragar" algo, você usa o 'with' aqui
            // Mas como o 'with' cria um novo objeto, você faz assim:
            return p;
        }

        // Para erros específicos:
        public ProdutoBRequest GerarRequestInvalido(string tipoErro)
        {
            var baseValida = GerarRequestValido();

            return tipoErro switch
            {
                "nome-vazio" => baseValida with { Nome = "" },
                "preco-negativo" => baseValida with { Preco = -1m },
                "descricao-longa" => baseValida with { Descricao = new string('x', 51) },
                _ => baseValida,
            };
        }
    }
}