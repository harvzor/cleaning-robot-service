using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Web.Dtos.Input;

namespace CleaningRobotService.Web.Mappers;

public static class CommandMapper
{
    public static CommandDto ToCommonCommandDto(this WebCommandDto command) => new()
    {
        Direction = command.Direction,
        Steps = command.Steps,
    };

    public static IEnumerable<CommandDto> ToCommonCommandDtos(this IEnumerable<WebCommandDto> commands)
        => commands.Select(command => command.ToCommonCommandDto());
}
