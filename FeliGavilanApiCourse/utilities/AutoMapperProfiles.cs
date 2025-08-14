using AutoMapper;
using FeliGavilanApiCourse.DTOs;
using FeliGavilanApiCourse.Entities;

namespace FeliGavilanApiCourse.Utilities;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Genre, GenreDTO>();
        CreateMap<CreateGenreDTO, Genre>();

        CreateMap<Actor, ActorDTO>();
        CreateMap<CreateActorDTO, Actor>()
            .ForMember(p=>p.Picture,options=>options.Ignore());
    }
}