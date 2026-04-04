using Cadastro.Dados;
using Cadastro.Dtos.LivroDtos;
using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Modelos;
using Cadastro.Modelos.Enums;
using Cadastro.Modelos.ValueObjects;
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
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Testes.TesteIntegraçao.LivrotestIntegracao;

[Collection("ToolsIntegraçao")]
public class TestIntegraçaoLivro_DELETE
{
    private readonly FactoryIntegraçao _factory;

    public TestIntegraçaoLivro_DELETE(FactoryIntegraçao factory)
    {
        _factory = factory;
        _factory.LimparBanco(); // Limpa o banco antes de cada teste para garantir um ambiente limpo
    }

    [Fact]
    public async Task DeletarLivro_Existente_RetornaOk()
    {
        // Arrange
        var client = _factory.CreateClient();
        var usuarioDono = _factory.GeradorUsuarios();

        var livroParaSalvar = new Livro(
            titulo: "Livro de Teste Final",
            autor: "Autor",
            preco: new ValorMonetario(10.0m),
            descricao: "Desc",
            categoria: CategoriaLivro.Ficcao,
            usuarioId: usuarioDono.Id,
            ativo: true,
            usuario: null
        );

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Usuarios.Add(usuarioDono);
            db.Livros.Add(livroParaSalvar);
            await db.SaveChangesAsync();
        }

        // Act
        var response = await client.DeleteAsync($"/api/livros/{livroParaSalvar.Id}");

        // Assert
        // 1. O contrato da API foi respeitado (Retornou 200)
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // 2. A PROVA REAL: O banco de dados está limpo
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var aindaExiste = await db.Livros.AnyAsync(l => l.Id == livroParaSalvar.Id);
            Assert.False(aindaExiste, "ERRO: O livro ainda consta no banco de dados!");
        }
    }

    [Fact]
    public async Task DeletarLivro_NaoExistente_RetornaNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var idInexistente = Guid.NewGuid();
        // Act
        var response = await client.DeleteAsync($"/api/livros/{idInexistente}");
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeletarLivro_IdInvalido_RetornarBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var idInvalido = "123"; // ID que não é um GUID válido
        // Act
        var response = await client.DeleteAsync($"/api/livros/{idInvalido}");
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeletarLivro_UsuarioNaoDono_RetornarBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var usuarioDono = _factory.GeradorUsuarios();
        var usuarioNaoDono = _factory.GeradorUsuarios();
        var livroParaSalvar = new Livro(
            titulo: "Livro de Teste Final",
            autor: "Autor",
            preco: new ValorMonetario(10.0m),
            descricao: "Desc",
            categoria: CategoriaLivro.Ficcao,
            usuarioId: usuarioDono.Id,
            ativo: true,
            usuario: null
        );
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Usuarios.Add(usuarioDono);
            db.Usuarios.Add(usuarioNaoDono);
            db.Livros.Add(livroParaSalvar);
            await db.SaveChangesAsync();
        }
        // Simula autenticação do usuário que NÃO é o dono do livro
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {usuarioNaoDono.Id}");
        // Act
        var response = await client.DeleteAsync($"/api/livros/{livroParaSalvar.Id}");
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}