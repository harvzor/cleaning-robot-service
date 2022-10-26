using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.Web.Dtos.Output;

namespace CleaningRobotService.Web.Mappers;

public static class ExecutionMapper
{
    public static CommandRobotDto ToDto(this Execution execution) => new()
    {
        Id = execution.Id,
        Commands = execution.Commands,
        Duration = execution.Duration,
        Result = execution.Result,
        Timestamp = execution.TimeStamp,
    };
}
