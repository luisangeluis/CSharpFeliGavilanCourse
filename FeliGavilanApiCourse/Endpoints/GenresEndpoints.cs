using Microsoft.AspNetCore.Http.HttpResults;
using FeliGavilanApiCourse.Repositories;
using Microsoft.AspNetCore.OutputCaching;
using FeliGavilanApiCourse.DTOs;


namespace FeliGavilanApiCourse.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
    {
        group.MapPost("/", Create);
        group.MapGet("/", GetGenres)
            .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genres-get"));
        group.MapGet("/{id:int}", GetById);
        group.MapPut("/{id:int}", Update);
        group.MapDelete("/{id:int}", Delete);

        return group;
    }

    static async Task<Ok<List<Genre>>> GetGenres(IGenresRepository repository)
    {
        var genres = await repository.GetAll();
        return TypedResults.Ok(genres);

    }

    static async Task<Results<Ok<Genre>, NotFound>> GetById(int id, IGenresRepository repository)
    {
        var genre = await repository.GetById(id);

        if (genre is null) return TypedResults.NotFound();

        return TypedResults.Ok(genre);
    }

    static async Task<Created<Genre>> Create(CreateGenreDTO createGenreDTO, IGenresRepository repository, IOutputCacheStore outputCacheStore)
    {
        var genre = new Genre
        {
            Name = createGenreDTO.Name
        };
        
        var id = await repository.Create(genre);

        await outputCacheStore.EvictByTagAsync("genres-get", default);

        return TypedResults.Created($"/genres/{id}", genre);
    }

    static async Task<Results<NotFound, NoContent>> Update(int id, Genre genre, IGenresRepository repository, IOutputCacheStore outputCacheStore)
    {
        var exists = await repository.Exists(id);

        if (!exists) return TypedResults.NotFound();

        genre.Id = id;

        await repository.Update(genre);
        await outputCacheStore.EvictByTagAsync("genres-get", default);
        return TypedResults.NoContent();
    }

    static async Task<Results<NotFound, NoContent>> Delete(int id, IGenresRepository repository, IOutputCacheStore outputCacheStore)
    {
        var exists = await repository.Exists(id);

        if (!exists) return TypedResults.NotFound();

        await repository.Delete(id);
        await outputCacheStore.EvictByTagAsync("genres-get", default);
        return TypedResults.NoContent();
    }
}