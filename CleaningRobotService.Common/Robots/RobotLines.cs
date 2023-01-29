using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Structures;

namespace CleaningRobotService.Common.Robots;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class RobotLines : BaseRobot, IRobot
{
    private int _count;
    private readonly List<Line> _lines;
    
    public RobotLines(Point startPoint, IEnumerable<DirectionStep> commands)
        : base(startPoint: startPoint, commands: commands)
    {
        _lines = new List<Line>(Commands.Count);
    }

    public void CalculatePointsVisited()
    {
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

        foreach (DirectionStep command in Commands.Where(command => command.Steps != 0))
        {
            Point start = currentPoint;
            
            for (int i = 0; i < command.Steps; i++)
            {
                switch (command.Direction)
                {
                    case DirectionEnum.North:
                        currentPoint.Y++;
                        AddPoint();
                        break;
                    case DirectionEnum.East:
                        currentPoint.X++;
                        AddPoint();
                        break;
                    case DirectionEnum.South:
                        currentPoint.Y--;
                        AddPoint();
                        break;
                    case DirectionEnum.West:
                        currentPoint.X--;
                        AddPoint();
                        break;
                    default:
                        throw new ArgumentException(
                            message: $"DirectionStep direction of {command.Direction} not covered.",
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
