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
    public async Task<int> Create(Genre genre)
    {
        // throw new NotImplementedException();
        context.Add(genre);
        await context.SaveChangesAsync();
        return genre.Id;
    }

    public async Task<Genre?> GetById(int id)
    {
        return await context.Genres.FirstOrDefaultAsync(g=>g.Id == id);
    }

    public async Task<List<Genre>> GetAll()
    {
        return await context.Genres.ToListAsync();
    }

}
