using AutoMapper;
using FilmeApis.Data.Dtos;
using FilmeApis.Models;

namespace FilmeApis.Profiles
{
    public class EnderecoProfile : Profile
    {
       public EnderecoProfile() 
        {
            CreateMap<CreateEnderecoDto, Endereco>();
            CreateMap<Endereco, ReadEnderecoDto>();
            CreateMap<UpadateEndereçoDto, Endereco>();
        }

    }
}
