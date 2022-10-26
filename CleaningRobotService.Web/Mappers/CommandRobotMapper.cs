using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.Web.Dtos.Output;

namespace CleaningRobotService.Web.Mappers;

public static class CommandRobotMapper
{
    public static CommandRobotDto ToDto(this CommandRobot commandRobot) => new()
    {
        Id = commandRobot.Id,
        StartPoint = commandRobot.StartPoint,
        Commands = commandRobot.Commands,
    };
}
