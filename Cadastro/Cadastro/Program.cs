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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using FluentValidation;

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
builder.Services.AddScoped<ITokenRepositorio, RepoToken>();
builder.Services.AddScoped<LoginUsuarioUseCase>();
builder.Services.AddScoped<RefreshTokenUsesCases>();
builder.Services.AddValidatorsFromAssemblyContaining<RefreshTokenValidator>();

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

app.UseAuthentication(); // 1. Verifica o Token (Quem é você?)
app.UseAuthorization();  // 2. Verifica as permissões (Você pode deletar?)

app.MapControllers();
app.MapGet("/ping", () => Results.Ok("pong")); // O nosso teste de sanidade
app.Run();

public partial class Program


{ } // Adicione esta linha para permitir testes de integração