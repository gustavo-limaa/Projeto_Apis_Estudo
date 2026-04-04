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
public class TestIntegraçaoLivro_PUT
{
    private readonly FactoryIntegraçao _factory;

    public TestIntegraçaoLivro_PUT(FactoryIntegraçao factory)
    {
        _factory = factory;
        _factory.LimparBanco(); // Limpa o banco antes de cada teste para garantir um ambiente limpo
    }
}