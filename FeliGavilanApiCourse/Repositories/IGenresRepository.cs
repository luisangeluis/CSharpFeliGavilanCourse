namespace FeliGavilanApiCourse.Repositories;

public interface IGenresRepository
{
    Task<Genre?> GetById(int id);
    Task<int> Create(Genre genre);
    Task<List<Genre>> GetAll();
}
