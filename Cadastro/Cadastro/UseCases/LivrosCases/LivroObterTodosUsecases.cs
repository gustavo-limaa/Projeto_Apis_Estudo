using Cadastro.Dtos.LivroDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.LivrosCases;

public class LivroObterTodosUsecases
{
    private readonly IRepositorioLivros _repoLivro;

    public LivroObterTodosUsecases(IRepositorioLivros repoLivro)
    {
        _repoLivro = repoLivro;
    }

    public async Task<Result<List<LivroResponseDto>>> ObterTodosAsync()
    {
        var livros = await _repoLivro.ObterTodosAsync();
        if (livros == null || !livros.Any())
        {
            return Result<List<LivroResponseDto>>.Failure("Nenhum livro encontrado.");
        }
        var resposta = livros.Select(l => l.ToResponseDto()).ToList();
        return Result<List<LivroResponseDto>>.Success(resposta);
    }
}