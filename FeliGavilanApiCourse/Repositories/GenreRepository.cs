using FeliGavilanApiCourse.Data;

namespace FeliGavilanApiCourse.Repositories;

public class GenresRepository: IGenresRepository
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
}
