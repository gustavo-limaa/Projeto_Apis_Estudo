using Cadastro.Dtos.LivroDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.LivrosCases
{
    public class LivroDeletarUseCases
    {
        private readonly IRepositorioLivros _repoLivro;
        private readonly IRepositorioUsuario _usuario;

        public LivroDeletarUseCases(IRepositorioLivros repoLivro)
        {
            _repoLivro = repoLivro;
        }

        // Adicione o parâmetro usuarioLogadoId
        public async Task<Result<LivroResponseDto>> ExecutarAsync(Guid id, Guid usuarioLogadoId)
        {
            if (id == Guid.Empty) return Result<LivroResponseDto>.Failure("ID inválido.");

            var livro = await _repoLivro.ObterPorIdAsync(id);

            if (livro == null)
                return Result<LivroResponseDto>.Failure("Livro não encontrado.");

            // A TRAVA: Compara o dono do livro com quem está tentando deletar
            if (livro.UsuarioId != usuarioLogadoId)
            {
                return Result<LivroResponseDto>.Failure("Você não tem permissão para deletar este livro.");
            }

            var respostaDto = livro.ToResponseDto();
            bool deletado = await _repoLivro.DeletarAsync(id);

            if (!deletado)
                return Result<LivroResponseDto>.Failure("Erro técnico ao tentar excluir.");

            return Result<LivroResponseDto>.Success(respostaDto);
        }
    }
}