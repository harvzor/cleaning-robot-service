using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.DataPersistence.Repositories;

public interface IRepository<TEntity> where TEntity : BaseModel
{
    TEntity? GetById(Guid id);
    
    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    void Save();
}
