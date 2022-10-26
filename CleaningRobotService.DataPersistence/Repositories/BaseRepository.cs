using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.DataPersistence.Repositories;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
{
    protected readonly ServiceDbContext Context;
    
    public BaseRepository(ServiceDbContext context)
    {
        Context = context;
    }

    public TEntity? GetById(Guid id)
    {
        return Context.Set<TEntity>().FirstOrDefault(x => x.Id == id);
    }

    public void Add(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().RemoveRange(entities);
    }

    public void Save()
    {
        // BUG: Will this save all contexts? Even models not owned by thus repository? Maybe UoW makes more sense.
        Context.SaveChanges();
    }
}