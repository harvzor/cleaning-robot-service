using System.Linq.Expressions;
using CleaningRobotService.DataPersistence.Models;
using Microsoft.EntityFrameworkCore;

namespace CleaningRobotService.DataPersistence.Repositories;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
{
    private readonly ServiceDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(ServiceDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public TEntity? GetById(Guid id)
    {
        return Query().FirstOrDefault(x => x.Id == id);
    }
    
    public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
    {
        return Query().Where(filter);
    }
    
    public IEnumerable<TEntity> QueryObjectGraph<TProperty>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TProperty>> includeChildren)
    {
        return Query()
            .Where(filter)
            .Include(includeChildren);
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public void Save()
    {
        // BUG: Will this save all contexts? Even models not owned by thus repository? Maybe UoW makes more sense.
        _context.SaveChanges();
    }
    
    protected IQueryable<TEntity> Query(bool includeDeleted = false)
    {
        IQueryable<TEntity> queryable = _dbSet.AsQueryable();
        
        if (!includeDeleted)
            queryable = queryable.Where(x => x.DeletedAt == null);
        
        return queryable;
    }
}
