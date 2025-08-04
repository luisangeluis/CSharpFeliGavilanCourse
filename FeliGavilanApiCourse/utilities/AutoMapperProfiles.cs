using AutoMapper;
using FeliGavilanApiCourse.DTOs;

namespace FeliGavilanApiCourse.Utilities;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Genre, GenreDTO>();
        CreateMap<CreateGenreDTO, Genre>();
    }
}