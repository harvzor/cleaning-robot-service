using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Structs;

namespace CleaningRobotService.Web.Dtos.Input;

public class Command
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

public class CommandRobotPostDto
{
    /// <summary>
    /// Starting location of the robot.
    /// </summary>
    public Point Start { get; set; }

    /// <summary>
    /// Actual commands the robot should follow.
    /// </summary>
    public List<Command> Commands { get; set; } = new();
}
