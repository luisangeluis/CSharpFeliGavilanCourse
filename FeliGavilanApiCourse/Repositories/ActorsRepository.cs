using FeliGavilanApiCourse.Data;
using FeliGavilanApiCourse.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeliGavilanApiCourse.Repositories;

public class ActorsReposity : IActorsRepository
{

    private readonly AppDbContext context;

    public ActorsReposity(AppDbContext context)
    {
        this.context = context;
    }
    public async Task<List<Actor>> GetAll()
    {
        return await context.Actors.ToListAsync();
    }

    public async Task<Actor?> GetById(int id)
    {
        return await context.Actors.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<int> Create(Actor actor)
    {
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        return actor.Id;
    }

    public async Task Update(Actor actor)
    {
        context.Actors.Update(actor);

        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        await context.Actors.Where(a => a.Id == id).ExecuteDeleteAsync();
    }

    public async Task<bool> Exists(int id)
    {
        return await context.Actors.AnyAsync(a => a.Id == id);
    }
}