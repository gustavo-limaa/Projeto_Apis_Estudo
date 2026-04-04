using Cadastro.Dtos.LivroDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.LivrosCases
{
    public class LivroCriarUseCases
    {
        private readonly IRepositorioLivros _repositorioLivro;
        private readonly IRepositorioUsuario _repositorioUsuario; // <--- Adiciona isso

        // No construtor, você pede os dois para o ASP.NET
        public LivroCriarUseCases(IRepositorioLivros repositorioLivro, IRepositorioUsuario repositorioUsuario)
        {
            _repositorioLivro = repositorioLivro;
            _repositorioUsuario = repositorioUsuario;
        }

        public async Task<Result<LivroResponseDto>> ExecutarAsync(LivroCreatDto dto)
        {
            // 1. Primeiro de tudo: Busca o usuário no banco
            var usuario = await _repositorioUsuario.ObterPorIdAsync(dto.UsuarioId);

            // 2. Valida se o usuário existe
            if (usuario == null)
                return Result<LivroResponseDto>.Failure("Usuário não encontrado.");

            // 3. Valida se ele está ativo (A trava que vai fazer seu teste passar!)
            if (!usuario.Ativo)
            {
                return Result<LivroResponseDto>.Failure("Usuário inativo não pode cadastrar livros.");
            }

            // 4. Se passou na validação, aí sim cria a entidade do livro
            var livro = dto.ToEntity();

            // 5. O Repositório de livros salva
            var sucesso = await _repositorioLivro.AdicionarAsync(livro);

            if (!sucesso)
                return Result<LivroResponseDto>.Failure("Erro ao persistir no banco.");

            // 6. Devolve o Response mapeado
            return Result<LivroResponseDto>.Success(livro.ToResponseDto());
        }
    }
}