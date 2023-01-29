using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.DataPersistence.Models;
using DirectionStep = CleaningRobotService.Common.Dtos.Input.DirectionStep;

namespace CleaningRobotService.BusinessLogic.Mappers;

public static class CommandMapper
{
    public static DirectionStep ToDto(this DataPersistence.Models.DirectionStep directionStep) => new()
    {
        Direction = directionStep.Direction,
        Steps = directionStep.Steps,
    };

    public static IEnumerable<DirectionStep> ToDtos(this IEnumerable<DataPersistence.Models.DirectionStep> commands)
        => commands.Select(x => x.ToDto());
    
    public static DataPersistence.Models.DirectionStep ToModel(this DirectionStep command) => new()
    {
        Direction = command.Direction,
        Steps = command.Steps,
    };

    public static IEnumerable<DataPersistence.Models.DirectionStep> ToModels(this IEnumerable<DirectionStep> commands)
        => commands.Select(x => x.ToModel());
}
