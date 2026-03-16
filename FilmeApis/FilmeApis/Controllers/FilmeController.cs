namespace FilmeApis.Controllers;

using AutoMapper;
using FilmeApis.Data;
using FilmeApis.Data.Dtos;
using FilmeApis.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;


[ApiController]
[Route("api/[controller]")]

public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;


    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    private static int id = 0;

    [HttpPost]
    public IActionResult AdicionarFilme([FromBody] CreateFilmeDtos filmedtos)
    {
        Filme filme = _mapper.Map<Filme>(filmedtos);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(BuscarFilmePorId),
            new { id = filme.Id }
            , filme);
    }
    [HttpGet]
    [HttpGet]
        [HttpGet]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes()
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes
            .Include(filme => filme.Sessoes)        // Entra na lista de sessões
            .ThenInclude(sessao => sessao.Cinema)  // Dentro de cada sessão, busca o Cinema
            .ToList());
    }
    
    [HttpGet("{id}")]


    public IActionResult BuscarFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
        if (filme == null) return NotFound(); 

        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }


    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id, [FromBody]
    UpdateFilmeDTO filmeDTO)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null)
        {
            NotFound();
        }
        _mapper.Map(filmeDTO, filme);
        _context.SaveChanges();
        return NoContent();

    }


    [HttpPatch("{id}")]
    public IActionResult UpdateParcialFilme(int id, JsonPatchDocument<
    UpdateFilmeDTO> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null)
        {
            return NotFound();
        }

        var FilmeParaAtualizar = _mapper.Map<UpdateFilmeDTO>(filme);

        patch.ApplyTo(FilmeParaAtualizar, ModelState);

        if (!TryValidateModel(FilmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(FilmeParaAtualizar, filme);

        _context.SaveChanges();

        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null)
        {
            return NotFound();
        }

        _context.Filmes.Remove(filme);

        _context.SaveChanges();

        return NoContent();
    }

 
   


}
