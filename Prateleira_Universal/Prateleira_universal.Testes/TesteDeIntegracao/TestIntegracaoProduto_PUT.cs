namespace Prateleira_Universal.Testes.TesteDeIntegracao
{
    [Collection("Produto Collection")]
    public class TestIntegracaoProduto_PUT
    {
        private readonly ProdutoTestsFixture _fixture;
        private readonly HttpClient _client;

        public TestIntegracaoProduto_PUT(ProdutoTestsFixture fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
        }

        [Fact]
        public async Task AtualizarProduto_DeveModificarNoBancoReal()
        {
            // --- ARRANGE ---
            var produtoParaCriar = _fixture.GerarRequestValido();
            var postResponse = await _client.PostAsJsonAsync("api/produtos", produtoParaCriar);
            postResponse.EnsureSuccessStatusCode();
            var produtoCriado = await postResponse.Content.ReadFromJsonAsync<ProdutoBResponse>();
            var idParaAtualizar = produtoCriado.ProdutoID;
            var produtoAtualizado = new ProdutoBRequest(
                Nome: "Nome Atualizado",
                Descricao: "Descrição Atualizada",
                Tipo: EnumCategoria.Erva_Mate,
                Especificacoes: new Dictionary<string, string> { { "Origem", "RS" } },
                Preco: 99.99m
            );
            // --- ACT ---
            var putResponse = await _client.PutAsJsonAsync($"api/produtos/{idParaAtualizar}", produtoAtualizado);
            putResponse.EnsureSuccessStatusCode();
            // --- ASSERT ---
            var getResponse = await _client.GetAsync($"/api/produtos/{idParaAtualizar}");
            getResponse.EnsureSuccessStatusCode();
            var produtoObtido = await getResponse.Content.ReadFromJsonAsync<ProdutoBResponse>();
            Assert.NotNull(produtoObtido);
            Assert.Equal(idParaAtualizar, produtoObtido.ProdutoID);
            Assert.Equal(produtoAtualizado.Nome, produtoObtido.Nome);
            Assert.Equal(produtoAtualizado.Descricao, produtoObtido.Descricao);
            Assert.Equal(produtoAtualizado.Tipo, produtoObtido.Tipo);
            Assert.Equal(produtoAtualizado.Preco, produtoObtido.Preco);
            Assert.Equal(produtoAtualizado.Especificacoes, produtoObtido.Especificacoes);
        }

        [Fact]
        public async Task AtualizarProduto_Inexistente_DeveRetornarNotFound()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();
            var produtoAtualizado = new ProdutoBRequest(
                Nome: "Nome Atualizado",
                Descricao: "Descrição Atualizada",
                Tipo: EnumCategoria.Erva_Mate,
                Especificacoes: new Dictionary<string, string> { { "Origem", "RS" } },
                Preco: 99.99m
            );
            // Act
            var putResponse = await _client.PutAsJsonAsync($"api/produtos/{idInexistente}", produtoAtualizado);
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, putResponse.StatusCode);
        }

        [Fact]
        public async Task AtualizarProduto_DadosInvalidos_DeveRetornarBadRequest()
        {
            // Arrange
            var produtoParaCriar = _fixture.GerarRequestValido();
            var postResponse = await _client.PostAsJsonAsync("api/produtos", produtoParaCriar);
            postResponse.EnsureSuccessStatusCode();
            var produtoCriado = await postResponse.Content.ReadFromJsonAsync<ProdutoBResponse>();
            var idParaAtualizar = produtoCriado.ProdutoID;
            ProdutoBRequest produtoAtualizado = null; // Dados inválidos (nulo)
            // Act
            var putResponse = await _client.PutAsJsonAsync($"api/produtos/{idParaAtualizar}", produtoAtualizado);
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, putResponse.StatusCode);
        }

        [Fact]
        public async Task AtualizarProduto_DadosInvalidos_EspecificacoesNulas_DeveRetornarBadRequest()
        {
            // Arrange
            var produtoParaCriar = _fixture.GerarRequestValido();
            var postResponse = await _client.PostAsJsonAsync("api/produtos", produtoParaCriar);
            postResponse.EnsureSuccessStatusCode();
            var produtoCriado = await postResponse.Content.ReadFromJsonAsync<ProdutoBResponse>();
            var idParaAtualizar = produtoCriado.ProdutoID;
            var produtoAtualizado = new ProdutoBRequest(
                Nome: "Nome Atualizado",
                Descricao: "Descrição Atualizada",
                Tipo: EnumCategoria.Erva_Mate,
                Especificacoes: null, // Especificações nulas (dados inválidos)
                Preco: 99.99m
            );
            // Act
            var putResponse = await _client.PutAsJsonAsync($"api/produtos/{idParaAtualizar}", produtoAtualizado);
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, putResponse.StatusCode);
        }

        [Fact]
        public async Task AtualizarProduto_DadosInvalidos_NomeVazio_DeveRetornarBadRequest()
        {
            // Arrange
            var produtoParaCriar = _fixture.GerarRequestValido();
            var postResponse = await _client.PostAsJsonAsync("api/produtos", produtoParaCriar);
            postResponse.EnsureSuccessStatusCode();
            var produtoCriado = await postResponse.Content.ReadFromJsonAsync<ProdutoBResponse>();
            var idParaAtualizar = produtoCriado.ProdutoID;
            var produtoAtualizado = new ProdutoBRequest(
                Nome: "", // Nome vazio (dados inválidos)
                Descricao: "Descrição Atualizada",
                Tipo: EnumCategoria.Erva_Mate,
                Especificacoes: new Dictionary<string, string> { { "Origem", "RS" } },
                Preco: 99.99m

            );
            // Act
            var putResponse = await _client.PutAsJsonAsync($"api/produtos/{idParaAtualizar}", produtoAtualizado);
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, putResponse.StatusCode);
        }
    }
}