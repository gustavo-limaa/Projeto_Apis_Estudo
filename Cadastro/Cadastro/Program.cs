using Cadastro.Dados;
using Cadastro.Dados.Repositorios;
using Cadastro.Intefaces;
using Cadastro.UseCases.LivrosCases;
using Cadastro.UseCases.LoginCases;
using Cadastro.UseCases.UsuarioCases;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// No Program.cs// 1. Pega a string de conexão das configurações
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Substitui o UseInMemoryDatabase por este bloco:
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString) // Deixa o Pomelo descobrir a versão do seu MySQL
    ));

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

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();