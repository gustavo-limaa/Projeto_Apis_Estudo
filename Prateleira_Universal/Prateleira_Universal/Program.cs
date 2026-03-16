using Microsoft.EntityFrameworkCore;
using Prateleira_Universal.Data.context;
using Prateleira_Universal.interfaces;
using Prateleira_Universal.Repositorios;
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

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
// No .NET 9+, o AddOpenApi já faz o trabalho de explorar os endpoints
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Gera o JSON
    app.MapScalarApiReference(); // Só isso! Sem parâmetros extras por enquanto
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();