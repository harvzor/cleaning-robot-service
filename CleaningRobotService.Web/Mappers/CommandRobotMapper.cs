using CleaningRobotService.BusinessLogic.Mappers;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.Web.Dtos.Output;

namespace CleaningRobotService.Web.Mappers;

public static class CommandRobotMapper
{
    public static CommandRobotDto ToDto(this CommandRobot commandRobot) => new()
    {
        Id = commandRobot.Id,
        StartPoint = commandRobot.StartPoint.ToDto(),
        Commands = commandRobot.Commands.ToDtos().ToList().AsReadOnly(),
        CreatedAt = commandRobot.CreatedAt,
        ModifiedAt = commandRobot.ModifiedAt,
        // TODO: if it's deleted, surely it should be returned.
        DeletedAt = commandRobot.DeletedAt,
    };
}
