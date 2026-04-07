using Cadastro;
using Cadastro.Controllers;
using Cadastro.Dados;
using Cadastro.Dados.Repositorios;
using Cadastro.Dtos.LoginDtos;
using Cadastro.Intefaces;
using Cadastro.UseCases.LivrosCases;
using Cadastro.UseCases.LoginCases;
using Cadastro.UseCases.LoginCases.Validacoes;
using Cadastro.UseCases.UsuarioCases;
using FluentValidation;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Mantenha APENAS esta chamada com o AddApplicationPart
builder.Services.AddControllers()
    .AddApplicationPart(typeof(LivrosController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// No Program.cs// 1. Pega a string de conexão das configurações
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    // Em vez de ServerVersion.AutoDetect(connectionString)
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 30)); // Ou a versão do seu MySQL
    options.UseMySql(connectionString, serverVersion);
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false; // Remove o header "Server" para ocultar informações sobre o servidor
});

// Add services to the container.
builder.Services.AddApplicationServices();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseHsts();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication(); // 1. Verifica o Token (Quem é você?)
app.UseAuthorization();  // 2. Verifica as permissões (Você pode deletar?)

app.MapControllers();
app.MapGet("/ping", () => Results.Ok("pong")); // O nosso teste de sanidade
app.Run();

public partial class Program


{ } // Adicione esta linha para permitir testes de integração