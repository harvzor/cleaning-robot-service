namespace CleaningRobotService.Web.Dtos.Output;

public class CommandRobotDto
{
    public int Id { get; set; }
    
    /// <inheritdoc cref="Models.Execution.TimeStamp"/>
    public DateTimeOffset Timestamp { get; set; }
    
    /// <inheritdoc cref="Models.Execution.Commands"/>
    public int Commands { get; set; }
    
    /// <inheritdoc cref="Models.Execution.Result"/>
    public int Result { get; set; }
    
    /// <inheritdoc cref="Models.Execution.Duration"/>
    public double Duration { get; set; }
}