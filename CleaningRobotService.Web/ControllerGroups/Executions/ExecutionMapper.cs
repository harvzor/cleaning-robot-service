using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.Web.Dtos.Output;

namespace CleaningRobotService.Web.ControllerGroups.Executions;

public static class ExecutionMapper
{
    public static ExecutionDto ToDto(this Execution execution) => new()
    {
        Id = execution.Id,
        Duration = execution.Duration,
        Result = execution.Result,
    };

    public static IEnumerable<ExecutionDto> ToDtos(this IEnumerable<Execution> executions) =>
        executions.Select(x => x.ToDto());
}
