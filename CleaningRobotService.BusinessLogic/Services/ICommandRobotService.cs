using CleaningRobotService.Common.Dtos;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.DataPersistence.Models;
using DirectionStep = CleaningRobotService.Common.Dtos.Input.DirectionStep;

namespace CleaningRobotService.BusinessLogic.Services;

public interface ICommandRobotService
{
    Command CreateCommandRobot(PointDto startPoint, IReadOnlyCollection<DirectionStep> commands, bool runExecutionAsync = false);
}
