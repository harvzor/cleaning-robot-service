namespace CleaningRobotService.DataPersistence.Models;

/// <summary>
/// A single task sent to the cleaning robot.
/// </summary>
public class Execution : BaseModel
{
    ///<summary>
    /// How many command elements were received.
    /// </summary>
    public int Commands { get; set; }
    
    /// <summary>
    /// Number of indices/points cleaned.
    /// </summary>
    public int? Result { get; set; }
    
    /// <summary>
    /// How long it took to calculate the <see cref="Result"/>.
    /// </summary>
    public TimeSpan? Duration { get; set; }
    
    /// <summary>
    /// Which <see cref="CommandRobot"/> this belongs to.
    /// </summary>
    public Guid CommandRobotId { get; set; }
    
    /// <summary>
    /// Which <see cref="CommandRobot"/> this belongs to.
    /// </summary>
    public virtual CommandRobot CommandRobot { get; set; }
}
