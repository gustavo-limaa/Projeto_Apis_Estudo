using Cadastro.Dtos.LivroDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.LivrosCases
{
    public class LivroDeletarUseCases
    {
        private readonly IRepositorioLivros _repoLivro;

        public LivroDeletarUseCases(IRepositorioLivros repoLivro)
        {
            _repoLivro = repoLivro;
        }

        public async Task<Result<LivroResponseDto>> ExecutarAsync(Guid id)
        {
            if (id == Guid.Empty) return Result<LivroResponseDto>.Failure("ID inválido.");

            // 1. Busca o livro antes de deletar para ter os dados (Nome, Autor, etc)
            var livro = await _repoLivro.ObterPorIdAsync(id);

            if (livro == null)
                return Result<LivroResponseDto>.Failure("Livro não encontrado para deleção.");

            // 2. Transforma em DTO agora (antes de deletar a entidade)
            var respostaDto = livro.ToResponseDto();

            // 3. Manda o Repositório apagar
            bool deletado = await _repoLivro.DeletarAsync(id);

            if (!deletado)
                return Result<LivroResponseDto>.Failure("Erro técnico ao tentar excluir o livro.");

            // 4. Retorna o DTO! Agora o Controller tem o Nome e o ID para avisar o usuário.
            return Result<LivroResponseDto>.Success(respostaDto);
        }
    }
}