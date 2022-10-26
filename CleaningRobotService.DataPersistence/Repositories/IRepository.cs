namespace CleaningRobotService.DataPersistence.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity? GetById(int id);
    
    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    void Save();
}
