using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_universal.Testes.TesteDeResilienciaMoq
{
    public class MoqIntegracao_put : IClassFixture<MockProdutoFactory>
    {
        private readonly MockProdutoFactory _factory;
        private readonly HttpClient _client;

        public MoqIntegracao_put(MockProdutoFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
    }
}