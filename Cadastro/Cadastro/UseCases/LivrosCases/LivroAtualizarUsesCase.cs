using Cadastro.Dtos.LivroDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.LivrosCases
{
    public class LivroAtualizarUsesCase

    {
        private readonly IRepositorioLivros _repositorio;

        public LivroAtualizarUsesCase(IRepositorioLivros repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<LivroResponseDto>> ExecutarAsync(LivroUpdateDto dto)
        {
            var livroExistente = await _repositorio.ObterPorIdAsync(dto.Id);

            if (livroExistente == null) return Result<LivroResponseDto>.Failure("Livro não encontrado.");
            // 1. Transforma o DTO em Entidade
            dto.MapToEntity(livroExistente);

            // 2. Chama o Repositório
            var sucesso = await _repositorio.AtualizarAsync(livroExistente);

            if (!sucesso)
                return Result<LivroResponseDto>.Failure("Não foi possível atualizar o livro.");

            // 3. Mapeia de volta para o DTO de Resposta (o que vai pro JSON)
            return Result<LivroResponseDto>.Success(livroExistente.ToResponseDto());
        }
    }
}