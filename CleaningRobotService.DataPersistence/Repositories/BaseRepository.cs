using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.DataPersistence.Repositories;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
{
    private readonly ServiceDbContext _context;
    
    public BaseRepository(ServiceDbContext context)
    {
        _context = context;
    }

    public TEntity? GetById(Guid id)
    {
        return _context.Set<TEntity>().FirstOrDefault(x => x.Id == id);
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }

    public void Save()
    {
        // BUG: Will this save all contexts? Even models not owned by thus repository? Maybe UoW makes more sense.
        _context.SaveChanges();
    }
}