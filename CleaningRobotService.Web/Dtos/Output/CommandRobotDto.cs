using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;

namespace CleaningRobotService.Web.Dtos.Output;

public class CommandRobotDto : BaseDto
{
    /// <summary>
    /// Starting location of the robot.
    /// </summary>
    public Point StartPoint { get; set; }

    /// <summary>
    /// Actual commands the robot should follow.
    /// </summary>
    public IReadOnlyCollection<CommandDto> Commands { get; set; } = Array.Empty<CommandDto>();
}
