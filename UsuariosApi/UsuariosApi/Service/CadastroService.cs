using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using UsuariosApi.data.Dtos;
using UsuariosApi.Modelos;
using UsuariosApi.Service;

using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace UsuariosApi.Service;

public class UsuarioService
{
    private IMapper _mapper;
    private UserManager<Usuario> _userManager;
    private SignInManager<Usuario> _signInManager;
    private TokenService _tokenService;
    public UsuarioService(IMapper mapper, UserManager<Usuario> userManager,
         SignInManager<Usuario> signInManger, TokenService tokenService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManger;
        _tokenService = tokenService;
    }



    public async Task Casdastra(CreateUsuarioDto usuarioDto)
    {

        Usuario usuario = _mapper.Map<Usuario>(usuarioDto);

        IdentityResult resultado = await
            _userManager.CreateAsync(usuario, usuarioDto.Password);

        if (!resultado.Succeeded)
        {
            throw new ApplicationException("Falha ao cadastrar usuário!");

        }
}

    public async Task<string> login(LoginUsuarioDto dto)
    {
       var resulta = await
            _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);
        
        if (!resulta.Succeeded)
        {
            throw new ApplicationException("O Usuario ou senha nao foram autemticados!");
        }
        var usuario = _signInManager.UserManager.Users.FirstOrDefault(user => user.NormalizedUserName == dto.Username.ToUpper());

        var token = _tokenService.GenerateToken(usuario);
        return token;
    }
}