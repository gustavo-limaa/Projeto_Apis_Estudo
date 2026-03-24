using Microsoft.EntityFrameworkCore;
using Prateleira_Universal.Adapters.Data.context;
using Prateleira_Universal.Adapters.Repositorios;
using Prateleira_Universal.Domain.interfaces;
using Prateleira_Universal.interfaces;
using Prateleira_Universal.interfaces.RefitApi;
using Prateleira_Universal.UseCases;
using Refit;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddRefitClient<IApiProduto>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7014/"));

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddScoped<CriarProdutoUseCase>();
builder.Services.AddScoped<AtualizarProdutoUseCase>();
builder.Services.AddScoped<DeletarProdutoUseCase>(); // O que estamos fazendo agora
builder.Services.AddScoped<ObterProdutoPorIdUseCase>();
builder.Services.AddScoped<ObterProdutosPorCategoriaUseCase>();
builder.Services.AddScoped<ObterTodosProdutosUseCase>();// No .NET 9+, o AddOpenApi já faz o trabalho de explorar os endpoints
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Gera o JSON
    app.MapScalarApiReference(); // Só isso! Sem parâmetros extras por enquanto
}
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program


{ } // Necessário para os testes de integração acessarem o WebApplicationFactory