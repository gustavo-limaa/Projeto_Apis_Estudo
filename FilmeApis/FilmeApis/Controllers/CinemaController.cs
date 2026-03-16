
using AutoMapper;
using FilmeApis.Data;
using FilmeApis.Data.Dtos;
using FilmeApis.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;


namespace FilmeApis.Controllers;

[ApiController]

[Route("api/[controller]")]
public class CinemaController : ControllerBase


{

    private FilmeContext _context;
    private IMapper _mapper;


 public   CinemaController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    private static int id = 0;

    [HttpPost]
    public IActionResult AdicionarCinema([FromBody] CreatCinemaDto cinemaDto)
    {
        Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
        cinema.Id = id++;


        _context.Cinemas.Add(cinema);
        _context.SaveChanges();


        return CreatedAtAction(nameof(BuscarCinemaPorId), new { id = cinema.Id }, cinema);
    }



    [HttpGet]
    public IActionResult BuscaCinema()
    {
        
        
        var listaDto = _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas
            .Include(cinema => cinema.Endereco)
            .Include(cinema => cinema.Sessoes)  
            .ToList());


        return Ok(listaDto);
    }

    [HttpGet("{id}")]
    public IActionResult BuscarCinemaPorId(int id)
    {
        Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);

        if (cinema == null)
        {
            return NotFound();
        }


        ReadCinemaDto cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);

        return Ok(cinemaDto);
    }


    [HttpPut("{id}")]

    public IActionResult AtualizarCinema(int id, [FromBody] UpdateCinemaDto cinemaDto)
    {
        Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
        if (cinema == null)
        {
            return NotFound();
        }
        _mapper.Map(cinemaDto, cinema);
        _context.SaveChanges();


        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult DeletarCinema(int id)
    {
        Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);

        if (cinema == null)
        {  return NotFound(); }

        _context.Cinemas.Remove(cinema);
        _context.SaveChanges();

        return NoContent(); //


    }
   
    






}
