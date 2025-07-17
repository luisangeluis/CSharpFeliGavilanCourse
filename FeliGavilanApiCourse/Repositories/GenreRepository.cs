using FeliGavilanApiCourse.Data;
using Microsoft.EntityFrameworkCore;

namespace FeliGavilanApiCourse.Repositories;

public class GenresRepository : IGenresRepository
{
    private readonly AppDbContext context;
    public GenresRepository(AppDbContext context)
    {
        this.context = context;
    }

     public async Task<List<Genre>> GetAll()
    {
        return await context.Genres.ToListAsync();
        // return await context.Genres.OrderBy(g=>g.Name).ToListAsync();
        // return await context.Genres.OrderByDescending(g=>g.Name).ToListAsync();
    }

    public async Task<Genre?> GetById(int id)
    {
        return await context.Genres.FirstOrDefaultAsync(g=>g.Id == id);
    }

    public async Task<int> Create(Genre genre)
    {
        // throw new NotImplementedException();
        context.Add(genre);
        await context.SaveChangesAsync();
        return genre.Id;
    }

    public async Task<bool> Exists(int id)
    {
        return await context.Genres.AnyAsync(g=>g.Id == id);
    }

    public async Task Update(Genre genre)
    {
        context.Update(genre);
        await context.SaveChangesAsync();
    }
}
