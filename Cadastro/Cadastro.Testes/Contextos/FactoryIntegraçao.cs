using Cadastro;
using Cadastro.Dados;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastro.Testes.Contextos;

using Bogus;
using Cadastro.Dtos.LivroDtos;
using Cadastro.Modelos;
using Cadastro.Modelos.Enums;
using Cadastro.Modelos.ValueObjects;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using IConfiguration = IConfiguration;

public class FactoryIntegraçao : WebApplicationFactory<Program>, IDisposable
{
    // O segredo é usar o CONFIGURE e não o WITH
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSolutionRelativeContentRoot("C:/Users/Samsung/source/APIS/Cadastro/Cadastro");
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.SetBasePath(AppContext.BaseDirectory)
                  .AddJsonFile("appsettings.json", optional: false)
                  .AddUserSecrets<FactoryIntegraçao>() // Isso aqui diz: "Busque os segredos desse projeto"
                  .AddEnvironmentVariables();
        });

        builder.ConfigureServices(services =>
        {
            // 1. Limpa o DbContext anterior
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            // 2. Agora pegamos a configuração que acabamos de "forçar" acima
            // Pega a string do JSON que acabamos de configurar
            var sp = services.BuildServiceProvider();
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("DefaultConnectionTest");

            services.AddDbContext<AppDbContext>(options =>
            {
                var versao = new MySqlServerVersion(new Version(8, 0, 30));
                options.UseMySql(connectionString, versao);
            });

            // A MÁGICA ACONTECE AQUI:
            var spFinal = services.BuildServiceProvider();
            using var scope = spFinal.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Se o banco 'Livraria_Testes_Auto' não existir, o EF cria ele e todas as tabelas!
            db.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }

    public void initlize()
    {
        LimparBanco();
    }

    public void LimparBanco()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // 1. Desliga as verificações
        db.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 0;");

        // 2. Usa DELETE em vez de TRUNCATE (O segredo para evitar o erro de FK)
        db.Database.ExecuteSqlRaw("DELETE FROM Livros;");
        db.Database.ExecuteSqlRaw("DELETE FROM Usuarios;");

        // 3. Opcional: Se quiser resetar os IDs para 1 (como o truncate faria)
        db.Database.ExecuteSqlRaw("ALTER TABLE Livros AUTO_INCREMENT = 1;");
        db.Database.ExecuteSqlRaw("ALTER TABLE Usuarios AUTO_INCREMENT = 1;");

        // 4. Religamos a segurança
        db.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 1;");
    }

    // Gerador de Usuários
    public Usuario GeradorUsuarios()
    {
        return new Faker<Usuario>("pt_BR")
            .RuleFor(u => u.Nome, f => f.Person.FullName)
            .RuleFor(u => u.Senha, f => f.Internet.Password(8))
            .RuleFor(u => u.Email, f => new ValueEmail(f.Internet.Email()))
            .RuleFor(u => u.Ativo, f => true)
            .Generate();
    }

    // Gerador de Request (O que o JSON da API espera)
    public LivroCreatDto GeradorLivroRequest(Guid usuarioId)
    {
        var faker = new Faker("pt_BR");
        return new LivroCreatDto(

            Titulo: faker.Commerce.ProductName(),
            Autor: faker.Person.FullName,
            Descricao: faker.Commerce.ProductDescription(),
            Categoria: faker.PickRandom<CategoriaLivro>(), // Cast para int se o DTO esperar int
            Preco: faker.Finance.Amount(10, 500), // Decimal puro para o JSON não quebrar
            UsuarioId: usuarioId
        );
    }

    public List<Usuario> geradorlistaUsuarios(int quantidade)
=> Enumerable.Range(1, quantidade).Select(_ => GeradorUsuarios()).ToList();

    public List<Usuario> GeradorListaUsuarios(int qtd) => Enumerable.Range(1, qtd).Select(_ => GeradorUsuarios()).ToList();

    // O "Pulo do Gato" para testar BadRequests
    public object GeradorDeFalhas()
    {
        return new
        {
            Nome = "", // Inválido: Required
            Senha = (string)null, // Inválido: Required
            Email = "email-invalido", // Vai estourar o Regex do ValueEmail ou DataAnnotation
            Preco = -10.50m // O seu ValorMonetario vai lançar ArgumentException
        };
    }
}