using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;

namespace CleaningRobotService.Common.Robots;

public abstract class BaseRobot
{
    public Point StartPoint { get; set; }
    public List<CommandDto> Commands { get; set; }

    protected BaseRobot(Point startPoint, IEnumerable<CommandDto> commands)
    {
        StartPoint = startPoint;
        Commands = commands.ToList();
    }
}
