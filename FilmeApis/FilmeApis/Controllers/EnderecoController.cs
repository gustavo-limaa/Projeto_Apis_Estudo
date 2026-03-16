using AutoMapper;
using FilmeApis.Data;
using FilmeApis.Data.Dtos;
using FilmeApis.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace FilmeApis.Controllers
{
    [ApiController]

    [Route("api/[controller]")]

    public class EnderecoController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;
        private static int id { get; set; }


       public EnderecoController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionarEndereco([FromBody] CreateEnderecoDto enderecoDto)
        {
            Endereco endereco = _mapper.Map<Endereco>(enderecoDto);

            _context.Endereços.Add(endereco);
            _context.SaveChanges();

            // Boa prática: Retorna o local onde o endereço pode ser consultado
            return CreatedAtAction(nameof(BuscarEnderecoPorId), new { id = endereco.Id }, endereco);
        }

        
        [HttpGet]
        public IEnumerable<ReadEnderecoDto> BuscarEndereco() // Adicione o tipo <ReadEnderecoDto>
        {
            // O .ToList() é essencial para o AutoMapper conseguir transformar a lista
            return _mapper.Map<List<ReadEnderecoDto>>(_context.Endereços.ToList());
        }



        [HttpGet("{id}")]
        public IActionResult BuscarEnderecoPorId(int id)
        {
            Endereco endereco = _context.Endereços.FirstOrDefault(e => e.Id == id);
            if (endereco == null) return NotFound(); 

            ReadEnderecoDto enderecoDto = _mapper.Map<ReadEnderecoDto>(endereco);
            return Ok(enderecoDto);
        }

        [HttpPut("{Id}")]
        public IActionResult AtualizarEndereço([FromBody]UpadateEndereçoDto endereçoDot)
        {
            Endereco endereco = _context.Endereços.FirstOrDefault(e => e.Id == id);


            if (endereco == null) return NotFound();


            _mapper.Map(endereçoDot, endereco);
            _context.SaveChanges();

            return NoContent();


        }
        [HttpDelete("{id}")]
        public IActionResult DeletaEndereco(int id)
        {
            var endereco = _context.Endereços.FirstOrDefault(e => e.Id == id);
            if (endereco == null) return NotFound();

            try
            {
                _context.Remove(endereco);
                _context.SaveChanges();
                return NoContent();
            }
            catch (DbUpdateException)
            {
               
                return BadRequest("Não é possível deletar este endereço porque ele está vinculado a um cinema.");
            }
        }
    }
}
