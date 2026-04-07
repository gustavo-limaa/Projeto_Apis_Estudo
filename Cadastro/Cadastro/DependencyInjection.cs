using FluentValidation;
using Cadastro.Intefaces;
using Cadastro.UseCases.LivrosCases;
using Cadastro.UseCases.LoginCases;
using Cadastro.UseCases.UsuarioCases;
using Cadastro.Dados.Repositorios;
using Cadastro.UseCases.LoginCases.Validacoes;

namespace Cadastro
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepositorioUsuario, RepoUsuario>();
            services.AddScoped<IRepositorioLivros, RepoLivro>();

            services.AddScoped<CriarUsuarioUseCase>();
            services.AddScoped<AtualizarUsuarioUseCases>();
            services.AddScoped<ObterPorIdUsuarioUseCases>();
            services.AddScoped<DeletarUsuarioUseCases>();
            services.AddScoped<ListarTodosUsuarioUseCases>();

            services.AddScoped<LoginUsuarioUseCase>();

            services.AddScoped<LivroCriarUseCases>();
            services.AddScoped<LivroAtualizarUsesCase>();
            services.AddScoped<LivroObterPorIdUseCases>();
            services.AddScoped<LivroDeletarUseCases>();
            services.AddScoped<LivroObterTodosUsecases>();

            services.AddScoped<ITokenRepositorio, RepoToken>();
            services.AddScoped<RefreshTokenUsesCases>();

            services.AddValidatorsFromAssemblyContaining<RefreshTokenValidator>();

            return services;
        }
    }
}