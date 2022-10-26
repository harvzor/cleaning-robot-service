using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.DataPersistence.Repositories;

public class CommandRobotRepository : BaseRepository<CommandRobot>, ICommandRobotRepository
{
    public CommandRobotRepository(ServiceDbContext context) : base(context)
    {
    }
}
