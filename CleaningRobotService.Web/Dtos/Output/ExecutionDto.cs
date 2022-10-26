namespace CleaningRobotService.Web.Dtos.Output;

public class ExecutionDto
{
    public Guid Id { get; set; }

    /// <inheritdoc cref="DataPersistence.Models.Execution.Result"/>
    public int? Result { get; set; }
    
    /// <inheritdoc cref="DataPersistence.Models.Execution.Duration"/>
    public TimeSpan? Duration { get; set; }
}
