using Cadastro.Dtos.LivroDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.LivrosCases
{
    public class LivroObterPorIdUseCases
    {
        private readonly IRepositorioLivros _repoLivro;

        public LivroObterPorIdUseCases(IRepositorioLivros repoLivro)
        {
            _repoLivro = repoLivro;
        }

        public async Task<Result<LivroResponseDto>> ExecutarAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Result<LivroResponseDto>.Failure("ID inválido.");
            }
            var livro = await _repoLivro.ObterPorIdAsync(id);

            if (livro == null)
            {
                return Result<LivroResponseDto>.Failure("Livro não encontrado.");
            }

            var resposta = livro.ToResponseDto();
            return Result<LivroResponseDto>.Success(resposta);
        }
    }
}