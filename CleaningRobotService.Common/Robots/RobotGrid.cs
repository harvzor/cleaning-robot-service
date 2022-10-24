using System.Drawing;
using CleaningRobotService.Common.Collections;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;

namespace CleaningRobotService.Common.Robots;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
/// <remarks>
/// <see cref="GetPointsVisited"/> will return the Points unordered.
/// </remarks>
public class RobotGrid : BaseRobot, IRobot
{
    private readonly GridExpandable _gridExpandable;
    
    public RobotGrid(Point startPoint, IEnumerable<CommandDto> commands)
        : base(startPoint: startPoint, commands: commands)
    {
        _gridExpandable = new GridExpandable();
    }
    
    public RobotGrid(Point startPoint, IEnumerable<CommandDto> commands, int gridWidth)
        : base(startPoint: startPoint, commands: commands)
    {
        _gridExpandable = new GridExpandable(gridWidth: gridWidth);
    }

    public void CalculatePointsVisited()
    {
        Point currentPoint = StartPoint;

        _gridExpandable.AddPoint(point: StartPoint);

        void AddPoint()
        {
            _gridExpandable.AddPoint(currentPoint);
        }

        foreach (CommandDto command in Commands.Where(command => command.Steps != 0))
        {
            for (int i = 0; i < command.Steps; i++)
            {
                switch (command.Direction)
                {
                    case DirectionEnum.north:
                        currentPoint.Y++;
                        AddPoint();
                        break;
                    case DirectionEnum.east:
                        currentPoint.X++;
                        AddPoint();
                        break;
                    case DirectionEnum.south:
                        currentPoint.Y--;
                        AddPoint();
                        break;
                    case DirectionEnum.west:
                        currentPoint.X--;
                        AddPoint();
                        break;
                    default:
                        throw new ArgumentException(
                            message: $"CommandDto direction of {command.Direction} not covered.",
                            paramName: nameof(Commands)
                        );
                }
            }
        }
    }

    public IEnumerable<Point> GetPointsVisited() => _gridExpandable.GetPoints();

    public int CountPointsVisited() => _gridExpandable.Count();
}