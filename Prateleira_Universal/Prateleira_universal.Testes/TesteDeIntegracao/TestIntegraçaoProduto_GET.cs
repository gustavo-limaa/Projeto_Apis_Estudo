using Microsoft.AspNetCore.Mvc;
using Prateleira_universal.Testes.ContextTest;
using Prateleira_Universal.Controllers;
using Prateleira_Universal.Data.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_Universal.Testes.TesteDeIntegracao
{
    [Collection("Produto Collection")]
    public class TestIntegraçaoProduto_GET
    {
        private readonly ProdutoTestsFixture _fixture;
        private readonly HttpClient _client;
    }
}