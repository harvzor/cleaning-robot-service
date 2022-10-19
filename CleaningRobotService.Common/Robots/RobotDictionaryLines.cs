using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Structures;

namespace CleaningRobotService.Common.Robots;

public enum Plane
{
    Horizontal = 0,
    Vertical = 1,
}

/// <summary>
/// This robot works best when many commands are being sent and each command has a lot of steps (>5).
/// </summary>
public class RobotDictionaryLines : IRobot
{
    private int _count = 0;
    
    /// <summary>
    /// Second Key is the x or y coordinate.
    /// </summary>
    private Dictionary<(Plane, int), List<Line>> _lines = new();
    
    public Point StartPoint { get; set; }
    public IEnumerable<CommandDto> Commands { get; set; } = Enumerable.Empty<CommandDto>();

    public void CalculatePointsVisited()
    {
        // Capacity here isn't likely the be the same as the number of commands if the commands cause the robot to
        // travel the same lines.
        _lines = new Dictionary<(Plane, int), List<Line>>(Commands.Count());
        
        Point currentPoint = StartPoint;

        void AddPoint()
        {
            bool pointAlreadyOnLine = false;

            if (_lines.TryGetValue((Plane.Horizontal, currentPoint.Y), out List<Line>? matchingHorizontalLines))
            {
                foreach (Line line in matchingHorizontalLines)
                {
                    if (line.Start.X < line.End.X
                            ? currentPoint.X >= line.Start.X && currentPoint.X <= line.End.X
                            : currentPoint.X <= line.Start.X && currentPoint.X >= line.End.X)
                    {
                        pointAlreadyOnLine = true;
                        break;
                    }
                }
            }
            
            if (!pointAlreadyOnLine && _lines.TryGetValue((Plane.Vertical, currentPoint.X), out List<Line>? matchingVerticalLines))
            {
                foreach (Line line in matchingVerticalLines)
                {
                    if (line.Start.Y < line.End.Y
                            ? currentPoint.Y >= line.Start.Y && currentPoint.Y <= line.End.Y
                            : currentPoint.Y <= line.Start.Y && currentPoint.Y >= line.End.Y)
                    {
                        pointAlreadyOnLine = true;
                        break;
                    }
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

            Line line = new Line
            {
                Start = start,
                End = currentPoint,
            };

            // TODO: move to different class
            {
                Plane plane = command.Direction is DirectionEnum.west or DirectionEnum.east
                    ? Plane.Horizontal
                    : Plane.Vertical;
                int key = plane == Plane.Horizontal
                    ? line.Start.Y
                    : line.Start.X;

                if (_lines.TryGetValue((plane, key), out List<Line>? lines))
                {
                    lines.Add(line);
                }
                else
                {
                    _lines
                        .Add((plane, key), new List<Line>
                        {
                            line,
                        });
                }
            }
        }
    }

    public IEnumerable<Point> GetPointsVisited()
    {
        return _lines
            .SelectMany(group => group.Value)
            .SelectMany(line => line.CalculatePoints())
            .Distinct()
            .ToList();
    }

    public int CountPointsVisited() => _count;
}
