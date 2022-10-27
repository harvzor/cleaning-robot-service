using System.Linq.Expressions;
using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.DataPersistence.Repositories;

public interface IRepository<TEntity> where TEntity : BaseModel
{
    TEntity? GetById(Guid id);

    IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter);

    IEnumerable<TEntity> QueryObjectGraph<TProperty>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TProperty>> includeChildren
    );
    
    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    void Save();
}
