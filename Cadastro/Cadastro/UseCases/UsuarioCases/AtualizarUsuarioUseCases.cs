using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos.Erro;
using Cadastro.Modelos.Mapper;

namespace Cadastro.UseCases.UsuarioCases
{
    public class AtualizarUsuarioUseCases
    {
        private readonly IRepositorioUsuario _repositorio;

        public AtualizarUsuarioUseCases(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<UsuarioResponseDto>> ExecutarAsync(Guid id, UsuarioUpdateDto dto)
        {
            // 1. Validações Iniciais (Lembre do 'return'!)
            if (id == Guid.Empty)
                return Result<UsuarioResponseDto>.Failure("ID inválido.");

            if (dto is null)
                return Result<UsuarioResponseDto>.Failure("Dados de atualização não fornecidos.");

            try
            {
                // 2. O Repositório faz a busca, o mapeamento e o SaveChanges
                var usuarioAtualizado = await _repositorio.AtualizarAsync(id, dto);

                if (usuarioAtualizado == null)
                    return Result<UsuarioResponseDto>.Failure("Usuário não encontrado.");

                // 3. RETORNO: Sucesso com o ResponseDto (o que o front-end quer ver)
                return Result<UsuarioResponseDto>.Success(usuarioAtualizado.ToResponseDto());
            }
            catch (Exception ex)
            {
                // Caso o novo e-mail no DTO seja inválido, o ValueObject lança erro e capturamos aqui
                return Result<UsuarioResponseDto>.Failure(ex.Message);
            }
        }
    }
}