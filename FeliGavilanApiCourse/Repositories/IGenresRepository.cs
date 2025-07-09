namespace FeliGavilanApiCourse.Repositories;

public interface IGenresRepository
{
    Task<int> Create(Genre genre);
}
