using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Structures;

namespace CleaningRobotService.Common.Robots;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class RobotLines : IRobot
{
    private int _count = 0;
    private List<Line> _lines = new();
    
    public Point StartPoint { get; set; }
    public IEnumerable<CommandDto> Commands { get; set; } = Enumerable.Empty<CommandDto>();

    public void CalculatePointsVisited()
    {
        _lines = new List<Line>(Commands.Count());
        
        Point currentPoint = StartPoint;

        void AddPoint()
        {
            bool pointAlreadyOnLine = false;
            foreach (Line line in _lines)
            {
                if (
                    // Check the x coordinate is between the 2 lines.
                    (line.Start.X < line.End.X
                        ? currentPoint.X >= line.Start.X && currentPoint.X <= line.End.X
                        : currentPoint.X <= line.Start.X && currentPoint.X >= line.End.X)
                    // Check the y coordinate is between the 2 lines.
                    && (line.Start.Y < line.End.Y
                        ? currentPoint.Y >= line.Start.Y && currentPoint.Y <= line.End.Y
                        : currentPoint.Y <= line.Start.Y && currentPoint.Y >= line.End.Y))
                {
                    pointAlreadyOnLine = true;
                    break;
                }
            }

            if (!pointAlreadyOnLine)
                _count++;
        }
        
        AddPoint();

        foreach (CommandDto command in Commands.Where(command => command.Steps != 0))
        {
            Point start = currentPoint;
            
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

            Line line = new Line(start: start, end: currentPoint);

            _lines.Add(line);
        }
    }

    public IEnumerable<Point> GetPointsVisited() => _lines
        .SelectMany(line => line.CalculatePoints())
        .Distinct();

    public int CountPointsVisited() => _count;
}
