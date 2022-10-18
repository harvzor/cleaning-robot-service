using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Interfaces;

namespace CleaningRobotService.Common.Objects;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
/// <remarks>
/// <see cref="GetPointsVisited"/> will return the Points unordered.
/// </remarks>
public class RobotGrid : IRobot
{
    private readonly GridExpandable _gridExpandable;
    public Point StartPoint { get; set; }
    public IEnumerable<CommandDto> Commands { get; set; } = Enumerable.Empty<CommandDto>();

    public RobotGrid()
    {
        _gridExpandable = new GridExpandable();
    }
    
    public RobotGrid(int gridWidth)
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

    public IEnumerable<Point> GetPointsVisited()
    {
        return _gridExpandable.GetPoints();
    }

    public int CountPointsVisited() => _gridExpandable.Count();
}