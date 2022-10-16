using System.Drawing;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Interfaces;

namespace CleaningRobotService.Web.Objects;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class RobotGrid : IRobot
{
    private readonly GridExpandable _gridExpandable;
    public Point StartPoint { get; set; }
    public IEnumerable<Command> Commands { get; set; } = Enumerable.Empty<Command>();

    public RobotGrid()
    {
        _gridExpandable = new GridExpandable();
    }
    
    public RobotGrid(int gridWidth)
    {
        _gridExpandable = new GridExpandable(gridWidth: gridWidth);
    }

    public IEnumerable<Point> CalculatePointsVisited()
    {
        Point currentPoint = StartPoint;

        _gridExpandable.AddPoint(point: StartPoint);

        void Step(uint steps, Action action)
        {
            for (int i = 0; i < steps; i++)
            {
                action();

                _gridExpandable.AddPoint(currentPoint);
            }
        }

        // For loops are supposed to be faster but I'm not gonna sacrifice readability for performance.
        foreach (var command in Commands.Where(command => command.Steps != 0))
        {
            switch (command.Direction)
            {
                case DirectionEnum.north:
                    Step(steps: command.Steps, action: () => currentPoint.Y++);
                    break;
                case DirectionEnum.east:
                    Step(steps: command.Steps, action: () => currentPoint.X++);
                    break;
                case DirectionEnum.south:
                    Step(steps: command.Steps, action: () => currentPoint.Y--);
                    break;
                case DirectionEnum.west:
                    Step(steps: command.Steps, action: () => currentPoint.X--);
                    break;
                default:
                    throw new ArgumentException(
                        message: $"Command direction of {command.Direction} not covered.",
                        paramName: nameof(Commands)
                    );
            }
        }

        return _gridExpandable.GetPoints();
    }
}