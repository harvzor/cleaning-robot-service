using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.DataPersistence.Repositories;

public interface IExecutionRepository : IRepository<Execution>
{
    public IReadOnlyCollection<Execution> GetByCommandRobotId(Guid id);
}
