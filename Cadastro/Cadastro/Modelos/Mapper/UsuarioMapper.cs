using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Modelos;

namespace Cadastro.Modelos.Mapper
{
    public static class UsuarioMapper
    {
        public static UsuarioResponseDto ToResponseDto(this Usuario usuario)
        {
            return new UsuarioResponseDto(
               usuario.Id,
                usuario.Nome!,
                usuario.Email!.Valor, // AQUI: Pegamos só a string do Value Object para o DTO
                usuario.DataCriacao,
                usuario.Ativo
            );
        }

        public static Usuario ToEntity(this UsuarioCreatDto requestDto)
        {
            return new Usuario
            {
                Nome = requestDto.Nome,
                Email = new ValueEmail(requestDto.Email),
                Senha = requestDto.Senha,
                Ativo = true
            };
        }

        public static void UpdateEntity(this Usuario usuario, UsuarioUpdateDto updateDto)
        {
            usuario.Nome = updateDto.Nome;
            usuario.Email = new ValueEmail(updateDto.Email);
            if (!string.IsNullOrEmpty(updateDto.Senha))
            {
                usuario.Senha = updateDto.Senha;
            }
        }
    }
}