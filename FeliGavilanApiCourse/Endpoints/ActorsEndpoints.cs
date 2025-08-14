using AutoMapper;
using FeliGavilanApiCourse.DTOs;
using FeliGavilanApiCourse.Entities;
// using FeliGavilanApiCourse.Entities;
using FeliGavilanApiCourse.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;


namespace FeliGavilanApiCourse.Endpoints;

public static class ActorsEndpoints
{

    public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetActors)
            .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("actors-get"));

        group.MapGet("/{id:int}", GetById);
        group.MapPost("/", Create);
        group.MapPut("/{id:int}", Update);
        group.MapDelete("/{id:int}", Delete);

        return group;
    }
    static async Task<Ok<List<ActorDTO>>> GetActors(IActorsRepository repository, IMapper mapper)
    {
        var actors = await repository.GetAll();
        var actorsDTO = mapper.Map<List<ActorDTO>>(actors);
        return TypedResults.Ok(actorsDTO);
    }

    static async Task<Results<Ok<ActorDTO>, NotFound>> GetById(int id, IActorsRepository repository, IMapper mapper)
    {
        var actor = await repository.GetById(id);

        if (actor is null) return TypedResults.NotFound();

        var actorDTO = mapper.Map<ActorDTO>(actor);

        return TypedResults.Ok(actorDTO);
    }

    static async Task<Created<ActorDTO>> Create(CreateActorDTO createActorDTO, IMapper mapper)
    {
        var actor = mapper.Map<Actor>(createActorDTO);


    }

    static async Task<Results<NotFound, NoContent>> Update(int id, CreateActorDTO createActorDTO, IActorsRepository repository, IMapper mapper,
     IOutputCacheStore outputCacheStore)
    {
        var exists = await repository.Exists(id);

        if (!exists) return TypedResults.NotFound();

        var actor = mapper.Map<Actor>(createActorDTO);

        actor.Id = id;

        await repository.Update(actor);
        await outputCacheStore.EvictByTagAsync("actors-get", default);
        return TypedResults.NoContent();
    }

    static async Task<Results<NotFound, NoContent>> Delete(int id, IActorsRepository repository, IOutputCacheStore outputCacheStore)
    {
        var exists = await repository.Exists(id);

        if (!exists) return TypedResults.NotFound();

        await repository.Delete(id);
        await outputCacheStore.EvictByTagAsync("actors-get", default);
        return TypedResults.NoContent();
    }
}