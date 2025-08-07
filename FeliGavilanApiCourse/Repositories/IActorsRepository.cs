using FeliGavilanApiCourse.Entities;

namespace FeliGavilanApiCourse.Repositories;

public interface IActorsRepository
{
    Task<List<Actor>> GetAll();
    Task<Actor?> GetById(int id);

    Task<int> Create(Actor actor);

    Task<bool> Exists(int id);

    Task Update(Actor actor);
    Task Delete(int id);
    
}