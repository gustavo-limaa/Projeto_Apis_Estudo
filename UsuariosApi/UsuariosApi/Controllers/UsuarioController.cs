using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using UsuariosApi.data.Dtos;
using UsuariosApi.Modelos;
using UsuariosApi.Service;

namespace UsuariosApi.Controllers;

[ApiController]
[Route("Api/[Controller]")]
public class UsuarioController : ControllerBase
{
   
    private UsuarioService _usuarioService;

    public UsuarioController (UsuarioService cadastroService)
    {
        _usuarioService = cadastroService;
    }

    [HttpPost("cadastro")]
    public async Task<IActionResult> CasdastraUsuario(CreateUsuarioDto usuarioDto)
    {
      await  _usuarioService.Casdastra(usuarioDto);

        return Ok("O Usuario foi cadastrado Com Susseço");
    }

    [HttpPost("login")]

    public async Task<IActionResult> Login(LoginUsuarioDto dto)
    {
     var token =  await  _usuarioService.login(dto);
        return Ok(token);
    }



}


