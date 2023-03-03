using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Messenger;

namespace CleaningRobotService.Web.Messages;

public class CommandMessage : BaseMessage
{
    /// <summary>
    /// Starting location of the robot.
    /// </summary>
    public Point StartPoint { get; set; }

    /// <summary>
    /// Actual commands the robot should follow.
    /// </summary>
    public IReadOnlyCollection<DirectionStep> Commands { get; set; } = Array.Empty<DirectionStep>();
}
