using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.DataPersistence.Repositories;

public class CommandRepository : BaseRepository<Command>, ICommandRepository
{
    public CommandRepository(ServiceDbContext context) : base(context)
    {
    }
}
