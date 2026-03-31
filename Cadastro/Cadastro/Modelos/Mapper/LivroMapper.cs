using Cadastro.Dtos.LivroDtos;
using Cadastro.Modelos.Enums;
using Cadastro.Modelos.ValueObjects;

namespace Cadastro.Modelos.Mapper
{
    public static class LivroMapper
    {
        public static LivroResponseDto ToResponseDto(this Livro livro)
        {
            return new LivroResponseDto(
                livro.Id,
                livro.Titulo!,
                livro.Autor!,
                livro.Preco.Valor, // AQUI: Pegamos só o decimal do Value Object para o DTO
                livro.Descricao,
                livro.Categoria.ToString(), // Convertendo enum para string
                livro.DataCriacao,
                livro.Ativo
            );
        }

        public static Livro ToEntity(this LivroCreatDto dto)
        {
            return new Livro
            {
                Titulo = dto.Titulo,
                Autor = dto.Autor,
                Descricao = dto.Descricao,
                Categoria = dto.Categoria ?? CategoriaLivro.NaoDefinido,
                UsuarioId = dto.UsuarioId,

                // AQUI: O nascimento do objeto de valor seguro
                Preco = new ValorMonetario(dto.Preco)
            };
        }

        public static void MapToEntity(this LivroUpdateDto dto, Livro livroExistente)
        {
            livroExistente.Id = dto.Id;
            livroExistente.Titulo = dto.Titulo;
            livroExistente.Autor = dto.Autor;
            livroExistente.Descricao = dto.Descricao;
            livroExistente.Ativo = dto.Ativo;

            // Tratando o Enum com segurança
            if (Enum.TryParse(dto.Categoria, out CategoriaLivro categoria))
                livroExistente.Categoria = categoria;

            // Substituindo o Value Object inteiro
            livroExistente.Preco = new ValorMonetario(dto.Preco);
        }
    }
}