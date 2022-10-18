using CleaningRobotService.Common.Enums;

namespace CleaningRobotService.Common.Dtos.Input;

public class CommandDto
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
