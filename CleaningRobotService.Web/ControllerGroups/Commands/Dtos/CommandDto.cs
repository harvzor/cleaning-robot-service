using CleaningRobotService.Common.Dtos;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Web.ControllerGroups.Base.Dtos;

namespace CleaningRobotService.Web.Dtos.Output;

public class CommandDto : BaseDto
{
    /// <summary>
    /// Starting location of the robot.
    /// </summary>
    public PointDto StartPoint { get; set; }

    /// <summary>
    /// Actual commands the robot should follow.
    /// </summary>
    public IReadOnlyCollection<DirectionStep> Commands { get; set; } = Array.Empty<Common.Dtos.Input.DirectionStep>();
}
