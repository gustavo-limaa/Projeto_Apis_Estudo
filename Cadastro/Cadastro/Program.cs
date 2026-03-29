using Cadastro.Dados;
using Cadastro.Intefaces;
using Cadastro.Repositorios;
using Cadastro.UseCases.LoginCases;
using Cadastro.UseCases.UsuarioCases;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// No Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("LivrariaEstudoDB"));

builder.Services.AddScoped<IRepositorioUsuario, RepoUsuario>();
builder.Services.AddScoped<CriarUsuarioUseCase>();
builder.Services.AddScoped<AtualizarUsuarioUseCases>();
builder.Services.AddScoped<ObterPorIdUsuarioUseCases>();
builder.Services.AddScoped<DeletarUsuarioUseCases>();
builder.Services.AddScoped<ListarTodosUsuarioUseCases>();
builder.Services.AddScoped<LoginUsuarioUseCase>();

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