using Cadastro.Dados;
using Cadastro.Dados.Repositorios;
using Cadastro.Intefaces;
using Cadastro.UseCases.LivrosCases;
using Cadastro.UseCases.LoginCases;
using Cadastro.UseCases.UsuarioCases;
using Microsoft.EntityFrameworkCore;
using Cadastro.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Mantenha APENAS esta chamada com o AddApplicationPart
builder.Services.AddControllers()
    .AddApplicationPart(typeof(LivrosController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// No Program.cs// 1. Pega a string de conexão das configurações
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    // Em vez de ServerVersion.AutoDetect(connectionString)
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 30)); // Ou a versão do seu MySQL
    options.UseMySql(connectionString, serverVersion);
});

builder.Services.AddScoped<IRepositorioUsuario, RepoUsuario>();
builder.Services.AddScoped<IRepositorioLivros, RepoLivro>();
builder.Services.AddScoped<CriarUsuarioUseCase>();
builder.Services.AddScoped<AtualizarUsuarioUseCases>();
builder.Services.AddScoped<ObterPorIdUsuarioUseCases>();
builder.Services.AddScoped<DeletarUsuarioUseCases>();
builder.Services.AddScoped<ListarTodosUsuarioUseCases>();
builder.Services.AddScoped<LoginUsuarioUseCase>();
builder.Services.AddScoped<LivroCriarUseCases>();
builder.Services.AddScoped<LivroAtualizarUsesCase>();
builder.Services.AddScoped<LivroObterPorIdUseCases>();
builder.Services.AddScoped<LivroDeletarUseCases>();
builder.Services.AddScoped<LivroObterTodosUsecases>();

// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/ping", () => Results.Ok("pong")); // O nosso teste de sanidade
app.Run();

public partial class Program


{ } // Adicione esta linha para permitir testes de integração