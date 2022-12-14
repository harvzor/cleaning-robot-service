using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;

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
    public IReadOnlyCollection<CommandDto> Commands { get; set; } = Array.Empty<CommandDto>();
}
