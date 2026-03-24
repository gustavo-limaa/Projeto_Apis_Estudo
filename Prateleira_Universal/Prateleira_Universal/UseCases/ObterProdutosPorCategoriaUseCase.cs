using System.Runtime.CompilerServices;

namespace Prateleira_Universal.UseCases
{
    public class ObterProdutosPorCategoriaUseCase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ObterProdutosPorCategoriaUseCase(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<ProdutoBResponse>> ExecutarAsync(EnumCategoria categoria)
        {
            // 1. Validação de Segurança (Fail First)
            if (!Enum.IsDefined(typeof(EnumCategoria), categoria))
            {
                throw new ArgumentException("Categoria inválida.");
            }

            // 2. Busca no Repositório (Note o nome da variável alterado para 'produtos')
            var produtos = await _produtoRepository.ObterPorCategoriaAsync(categoria);

            // 3. Mapeamento para o DTO de Resposta (Usando o seu Mapper!)
            // Se a lista vier vazia, o Select retorna uma lista vazia, o que é correto (200 OK [])
            return produtos.Select(p => p.ToResponse()).ToList();
        }
    }
}