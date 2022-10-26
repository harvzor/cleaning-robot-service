using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.Web.Dtos.Output;

namespace CleaningRobotService.Web.Mappers;

public static class ExecutionMapper
{
    public static ExecutionDto ToDto(this Execution execution) => new()
    {
        Id = execution.Id,
        Duration = execution.Duration,
        Result = execution.Result,
    };
}
