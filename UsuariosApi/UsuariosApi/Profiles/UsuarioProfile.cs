using AutoMapper;
using UsuariosApi.data.Dtos;
using UsuariosApi.Modelos;

namespace UsuariosApi.Profiles;

public class UsuarioProfile : Profile

{
    public UsuarioProfile() 
    {
        CreateMap<CreateUsuarioDto, Usuario>();
    }
}
