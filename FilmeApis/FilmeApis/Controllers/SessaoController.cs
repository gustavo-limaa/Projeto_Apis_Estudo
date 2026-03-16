using AutoMapper;
using FilmeApis.Data;
using FilmeApis.Data.Dtos;
using FilmeApis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmeApis.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class SessaoController : ControllerBase
        {
            private FilmeContext _context;
            private IMapper _mapper;

            public SessaoController(FilmeContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

        [HttpPost]
        public IActionResult AdicionaSessao([FromBody] CreateSessaoDto dto)
        {
           
            var filme = _context.Filmes.FirstOrDefault(f => f.Id == dto.FilmeId);
            var cinema = _context.Cinemas.FirstOrDefault(c => c.Id == dto.CinemaId);

            if (filme == null || cinema == null)
            {
               
                return BadRequest("Filme ou Cinema não encontrado no sistema.");
            }

            Sessao sessao = _mapper.Map<Sessao>(dto);
            _context.Sessoes.Add(sessao);
            _context.SaveChanges();

            return CreatedAtAction(nameof(RecuperaSessoesPorId), new { id = sessao.Id }, sessao);
        }

        [HttpGet]
            public IEnumerable<ReadSessaoDto> RecuperaSessoes()
            {
              
                return _mapper.Map<List<ReadSessaoDto>>(_context.Sessoes
                    .Include(s => s.Filme)
                    .Include(s => s.Cinema)
                    .ToList());
            }
        [HttpGet("{id}")]
        public IActionResult RecuperaSessoesPorId(int id)
        {
           
            var sessao = _context.Sessoes
                .Include(s => s.Filme)  
                .Include(s => s.Cinema) 
                .FirstOrDefault(s => s.Id == id);

            if (sessao == null) return NotFound();

          
            var sessaoDto = _mapper.Map<ReadSessaoDto>(sessao);

            return Ok(sessaoDto);
        }

        [HttpDelete("{id}")]
            public IActionResult DeletaSessao(int id)
            {
                var sessao = _context.Sessoes.FirstOrDefault(s => s.Id == id);
                if (sessao == null) return NotFound();

                _context.Sessoes.Remove(sessao);
                _context.SaveChanges();
                return NoContent();
            }
        }
    
}
