using CleaningRobotService.Common.Enums;

namespace CleaningRobotService.DataPersistence.Models;

/// <summary>
/// A single task sent to the cleaning robot.
/// </summary>
public class CommandRobotCommands : BaseModel
{
    /// <summary>
    /// Which direction the robot should move.
    /// </summary>
    public DirectionEnum Direction { get; set; }
    
    /// <summary>
    /// How many steps to take.
    /// </summary>
    public uint Steps { get; set; }
}
