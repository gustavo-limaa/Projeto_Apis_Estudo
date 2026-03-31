using Cadastro.Dtos.LivroDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.LivrosCases
{
    public class LivroCriarUseCases
    {
        private readonly IRepositorioLivros _repositorio;

        public LivroCriarUseCases(IRepositorioLivros repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<LivroResponseDto>> ExecutarAsync(LivroCreatDto dto)
        {
            // 1. O Mapper entra aqui para criar a entidade
            var livro = dto.ToEntity();

            // 2. O Repositório salva
            var sucesso = await _repositorio.AdicionarAsync(livro);

            if (!sucesso)
                return Result<LivroResponseDto>.Failure("Erro ao persistir no banco.");

            // 3. O Mapper entra de novo para devolver o Response
            return Result<LivroResponseDto>.Success(livro.ToResponseDto());
        }
    }
}