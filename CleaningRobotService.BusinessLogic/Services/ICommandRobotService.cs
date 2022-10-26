using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.BusinessLogic.Services;

public interface ICommandRobotService
{
    Execution CreateCommandRobot(Point startPoint, IReadOnlyCollection<CommandDto> commands, bool runExecutionAsync = false);
}
