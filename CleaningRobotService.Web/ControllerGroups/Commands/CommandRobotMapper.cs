using CleaningRobotService.BusinessLogic.Mappers;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.Web.Dtos.Output;

namespace CleaningRobotService.Web.ControllerGroups.Commands;

public static class CommandRobotMapper
{
    public static CommandDto ToDto(this Command command) => new()
    {
        Id = command.Id,
        StartPoint = command.StartPoint.ToDto(),
        Commands = command.DirectionSteps.ToDtos().ToList().AsReadOnly(),
        CreatedAt = command.CreatedAt,
        ModifiedAt = command.ModifiedAt,
        // TODO: if it's deleted, surely it should be returned.
        DeletedAt = command.DeletedAt,
    };
}
