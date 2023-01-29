using CleaningRobotService.Common.Dtos;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.BusinessLogic.Services;

public interface ICommandRobotService
{
    CommandRobot CreateCommandRobot(PointDto startPoint, IReadOnlyCollection<CommandDto> commands, bool runExecutionAsync = false);
}
