namespace FeliGavilanApiCourse.Repositories;

public interface IGenresRepository
{
    Task<List<Genre>> GetAll();
    Task<Genre?> GetById(int id);
    Task<int> Create(Genre genre);
    Task<bool> Exists(int id);
    Task Update(Genre genre);
}
