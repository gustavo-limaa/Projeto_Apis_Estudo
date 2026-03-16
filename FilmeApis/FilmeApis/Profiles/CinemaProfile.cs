using AutoMapper;
using FilmeApis.Data.Dtos;
using FilmeApis.Models;
    namespace FilmeApis.Profiles;

public class CinemaProfile : Profile
{
    public CinemaProfile() 
    {
        CreateMap<CreatCinemaDto, Cinema>();
        
        
        CreateMap<Cinema, ReadCinemaDto>()
            .ForMember(cinemaDto => cinemaDto.Endereco,otp
            => otp.MapFrom(cinema => cinema.Endereco));


        CreateMap<UpdateCinemaDto, Cinema>();


    }
}
