using CleaningRobotService.Messenger;

namespace CleaningRobotService.RobotCommander.Messages;

public class CommandResultMessage : BaseMessage
{
    /// <summary>
    /// Which <see cref="Command"/> this belongs to.
    /// </summary>
    public Guid CommandId { get; set; }
    
    public TimeSpan? CalculationTime { get; set; }
    
    public int? PointsVisited { get; set; }
}
