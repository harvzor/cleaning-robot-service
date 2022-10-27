using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.DataPersistence.Repositories;

public class ExecutionRepository : BaseRepository<Execution>, IExecutionRepository
{
    public ExecutionRepository(ServiceDbContext context) : base(context)
    {
    }

    public IReadOnlyCollection<Execution> GetByCommandRobotId(Guid id)
    {
        return Query()
            .Where(x => x.CommandRobotId == id)
            .ToList()
            .AsReadOnly();
    }
}
