using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.DataPersistence.Repositories;

// TODO: inherit from base repository since most of these methods will be the same
public class ExecutionRepository : IExecutionRepository
{
    private readonly ServiceDbContext _context;
    
    public ExecutionRepository(ServiceDbContext context)
    {
        _context = context;
    }

    public Execution? GetById(int id)
    {
        return _context.Executions.FirstOrDefault(x => x.Id == id);
    }

    public void Add(Execution entity)
    {
        _context.Executions.Add(entity);
    }

    public void AddRange(IEnumerable<Execution> entities)
    {
        _context.Executions.AddRange(entities);
    }

    public void Remove(Execution entity)
    {
        _context.Executions.Remove(entity);
    }

    public void RemoveRange(IEnumerable<Execution> entities)
    {
        _context.Executions.RemoveRange(entities);
    }

    public void Save()
    {
        // BUG: Will this save all contexts? Even models not owned by thus repository? Maybe UoW makes more sense.
        _context.SaveChanges();
    }
}