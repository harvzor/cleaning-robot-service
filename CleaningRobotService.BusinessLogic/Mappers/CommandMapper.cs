using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.DataPersistence.Models;

namespace CleaningRobotService.BusinessLogic.Mappers;

public static class CommandMapper
{
    public static CommandDto ToDto(this CommandRobotCommand commandRobotCommand) => new()
    {
        Direction = commandRobotCommand.Direction,
        Steps = commandRobotCommand.Steps,
    };

    public static IEnumerable<CommandDto> ToDtos(this IEnumerable<CommandRobotCommand> commands)
        => commands.Select(x => x.ToDto());
    
    public static CommandRobotCommand ToModel(this CommandDto command) => new()
    {
        Direction = command.Direction,
        Steps = command.Steps,
    };

    public static IEnumerable<CommandRobotCommand> ToModels(this IEnumerable<CommandDto> commands)
        => commands.Select(x => x.ToModel());
}
