using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using UsuariosApi.data.Dtos;
using UsuariosApi.Modelos;
using UsuariosApi.Service;



namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AcessoController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy =  "IdadeMinima")]
        public IActionResult Get()
        {
            return Ok("Acesso Libirado");

        }
    }
}
