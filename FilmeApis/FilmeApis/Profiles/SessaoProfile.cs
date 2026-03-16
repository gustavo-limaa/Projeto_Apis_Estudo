using AutoMapper;
using FilmeApis.Data.Dtos;
using FilmeApis.Models;

namespace FilmeApis.Profiles
{
    public class SessaoProfile : Profile
    {
        public SessaoProfile() 
        {
            CreateMap<CreateSessaoDto, Sessao>();

            CreateMap<Sessao, ReadSessaoDto>()
        .ForMember(dto => dto.Cinema, opt => opt.MapFrom(sessao => sessao.Cinema))
        .ForMember(dto => dto.Filme, opt => opt.MapFrom(sessao => sessao.Filme));
        }

    }
}
