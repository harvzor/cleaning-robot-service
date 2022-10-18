using CleaningRobotService.Common.Enums;

namespace CleaningRobotService.Web.Dtos.Input;

public class WebCommandDto
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
