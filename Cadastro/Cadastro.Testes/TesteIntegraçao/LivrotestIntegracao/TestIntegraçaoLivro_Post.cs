using Cadastro.Dados;
using Cadastro.Dtos.LivroDtos;
using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Modelos;
using Cadastro.Modelos.Enums;
using Cadastro.Testes.Contextos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Xunit.Sdk;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cadastro.Testes.TesteIntegraçao.LivrotestIntegracao;

[Collection("ToolsIntegraçao")]
public class TestIntegraçaoLivro_Post
{
    private readonly FactoryIntegraçao _factory;

    public TestIntegraçaoLivro_Post(FactoryIntegraçao factory)
    {
        _factory = factory;
        _factory.LimparBanco(); // Limpa o banco antes de cada teste para garantir um ambiente limpo
    }

    [Fact]
    public async Task CriarLivro_RetornaSucesso()
    {
        // Arrange
        var client = _factory.CreateClient();

        // 1. Cria e salva o usuário dono do livro no banco de teste
        var usuarioDono = _factory.GeradorUsuarios();
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Usuarios.Add(usuarioDono);
            await db.SaveChangesAsync();
        }

        // 2. Usa o NOVO gerador de Request (DTO) que já vem limpo para o JSON
        var request = _factory.GeradorLivroRequest(usuarioDono.Id);

        // ... Act ...
        var response = await client.PostAsJsonAsync("/api/livros", request);

        // Assert
        response.EnsureSuccessStatusCode();

        var livroCriado = await response.Content.ReadFromJsonAsync<LivroResponseDto>();

        // Verificações que fazem sentido para o seu Response:
        Assert.NotNull(livroCriado);
        Assert.NotEqual(Guid.Empty, livroCriado.Id); // Garante que o banco gerou um ID real

        // Validamos se o que foi salvo é o que enviamos (pelo título e autor)
        Assert.Equal(request.Titulo, livroCriado.Titulo);
        Assert.Equal(request.Autor, livroCriado.Autor);
    }

    [Fact]
    public async Task CriarLivro_SemUsuarioDono_RetornaErro()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Geramos um request com um usuárioId que NÃO existe no banco
        var request = _factory.GeradorLivroRequest(Guid.NewGuid()); // ID aleatório que não existe
        // Act
        var response = await client.PostAsJsonAsync("/api/livros", request);
        // Assert
        Assert.False(response.IsSuccessStatusCode); // Esperamos um erro (400 ou 404, dependendo da implementação)
    }

    [Fact]
    public async Task CriarLivro_DadosInvalidos_RetornaErro()
    {
        // Arrange
        var client = _factory.CreateClient();
        var usuarioDono = _factory.GeradorUsuarios();
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Usuarios.Add(usuarioDono);
            await db.SaveChangesAsync();
        }
        // Geramos um request com dados inválidos (ex: título vazio)
        var request = _factory.GeradorDeFalhas(
        );
        // Act
        var response = await client.PostAsJsonAsync("/api/livros", request);
        // Assert
        Assert.False(response.IsSuccessStatusCode); // Esperamos um erro de validação (400)
    }

    [Fact]
    public async Task CriarLivro_PrecoNegativo_RetornaErro()
    {
        // Arrange
        var client = _factory.CreateClient();
        var usuarioDono = _factory.GeradorUsuarios();
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Usuarios.Add(usuarioDono);
            await db.SaveChangesAsync();
        }
        // Geramos um request com preço negativo
        var request = _factory.GeradorDeFalhas();
        // Preço inválido
        // Act
        var response = await client.PostAsJsonAsync("/api/livros", request);
        // Assert
        Assert.False(response.IsSuccessStatusCode); // Esperamos um erro de validação (400)
    }

    [Fact]
    public async Task CriarLivro_UsuarioInativo_RetornaErro()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Dentro do seu teste
        var usuarioInativo = _factory.GeradorUsuarios();
        usuarioInativo.Ativo = false;

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Usuarios.Add(usuarioInativo);
            await db.SaveChangesAsync(); // ISSO AQUI É VITAL
        }

        // Garanta que o request use o ID que ACABOU de ser salvo
        var request = _factory.GeradorLivroRequest(usuarioInativo.Id);

        // Act
        var response = await client.PostAsJsonAsync("/api/livros", request);
        // Assert
        Assert.False(response.IsSuccessStatusCode); // Esperamos um erro (400 ou 403, dependendo da implementação)
    }

    [Fact]
    public async Task CriarLivro_SemAutenticacao_RetornaErro()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = _factory.GeradorLivroRequest(Guid.NewGuid()); // ID aleatório que não existe
        // Act
        var response = await client.PostAsJsonAsync("/api/livros", request);
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode); // Esperamos 401 Unauthorized
    }
}