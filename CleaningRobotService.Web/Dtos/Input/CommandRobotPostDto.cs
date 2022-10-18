using System.Drawing;

namespace CleaningRobotService.Web.Dtos.Input;

public class CommandRobotPostDto
{
    /// <summary>
    /// Starting location of the robot.
    /// </summary>
    public Point Start { get; set; }

    /// <summary>
    /// Actual commands the robot should follow.
    /// </summary>
    public List<WebCommandDto> Commands { get; set; } = new();
}
