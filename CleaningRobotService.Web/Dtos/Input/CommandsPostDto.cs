using CleaningRobotService.Common.Dtos;
using CleaningRobotService.Common.Dtos.Input;

namespace CleaningRobotService.Web.Dtos.Input;

public class CommandsPostDto
{
    /// <summary>
    /// Starting location of the robot.
    /// </summary>
    public PointDto Start { get; set; }

    /// <summary>
    /// Actual commands the robot should follow.
    /// </summary>
    public IReadOnlyCollection<DirectionStep> Commands { get; set; } = Array.Empty<DirectionStep>();
}
