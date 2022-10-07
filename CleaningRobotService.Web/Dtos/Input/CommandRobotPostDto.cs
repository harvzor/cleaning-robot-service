namespace CleaningRobotService.Web.Dtos.Input;

/// <summary>
/// A single location on a 2D plane the robot could be at.
/// </summary>
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

/// <summary>
/// Cardinal directions the robot could take.
/// </summary>
public enum DirectionEnum
{
    North = 0,
    East = 1,
    South = 2,
    West = 3,
}

public class Command
{
    /// <summary>
    /// Which direction the robot should move.
    /// </summary>
    public DirectionEnum Direction { get; set; }
    
    /// <summary>
    /// How many steps to take.
    /// </summary>
    public int Steps { get; set; }
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
    public List<Command> Commands { get; set; }
}