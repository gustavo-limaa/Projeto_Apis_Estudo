using Cadastro.Dados;
using Cadastro.Dtos.LivroDtos;
using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Testes.Contextos;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace Cadastro.Testes.TesteIntegraçao.LivrotestIntegracao;

[Collection("ToolsIntegraçao")]
public class TestIntegraçaoLivro_GeT
{
    private readonly FactoryIntegraçao _factory;

    public TestIntegraçaoLivro_GeT(FactoryIntegraçao factory)
    {
        _factory = factory;
        _factory.LimparBanco(); // Limpa o banco antes de cada teste para garantir um ambiente limpo
    }
}