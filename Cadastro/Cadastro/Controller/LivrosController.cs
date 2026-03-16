using Cadastro.Dados;
using Cadastro.Dtos.LivroDtos;
using Cadastro.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LivrosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var livros = await _context.Livros.ToListAsync();

            if (livros is not null && livros.Any())
            {
                var resposta = livros.Select(l =>
                {
                    string titulo = l.Titulo!;
                    return new LivroResponseDto(
                                    l.Id,
                                    titulo,
                                    l.Autor!,
                                    l.Preco,
                                    l.Descricao,
                                    l.DataCriacao,
                                    l.Ativo
                                    );
                }).ToList();

                return Ok(resposta);
            }
            return NotFound("Nenhum livro encontrado.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro is not null)
            {
                var resposta = new LivroResponseDto(
                    livro.Id,
                    livro.Titulo!,
                    livro.Autor!,
                    livro.Preco,
                    livro.Descricao,
                    livro.DataCriacao,
                    livro.Ativo
                );
                return Ok(resposta);
            }
            return NotFound("Livro não encontrado.");
        }

        [HttpPost]
        public async Task<IActionResult> Criar(LivroCreatDto dto)
        {
            // 1. Transformando o DTO na Entity (Entrada)
            var novoLivro = new Livro
            {
                Titulo = dto.Titulo,
                Autor = dto.Autor,
                Preco = dto.Preco,
                Descricao = dto.Descricao, // Não esqueça da descrição se ela existir no DTO!
                UsuarioId = dto.UsuarioId,
                Ativo = true
            };

            _context.Livros.Add(novoLivro);
            await _context.SaveChangesAsync(); // A mágica da data e do Guid acontece aqui!

            // 2. Transformando a Entity salva no ResponseDto (Saída)
            // Agora o objeto 'novoLivro' já tem ID e DataCriacao preenchidos pelo banco/contexto
            var resposta = new LivroResponseDto(
                novoLivro.Id,
                novoLivro.Titulo,
                novoLivro.Autor,
                novoLivro.Preco,
                novoLivro.Descricao,
                novoLivro.DataCriacao,
                novoLivro.Ativo
            );

            // 3. Retorno Profissional
            return CreatedAtAction(nameof(ObterPorId), new { id = resposta.Id }, resposta);
        }

        [HttpDelete]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro is not null)
            {
                _context.Livros.Remove(livro);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound("Livro não encontrado.");
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar(LivroUpdateDto dto)
        {
            var livro = await _context.Livros.FindAsync(dto.Id);

            if (livro is not null)
            {
                livro.Titulo = dto.Titulo;
                livro.Autor = dto.Autor;
                livro.Preco = dto.Preco;
                livro.Descricao = dto.Descricao;
                livro.Ativo = dto.Ativo;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound("Livro não encontrado.");
        }
    }
}