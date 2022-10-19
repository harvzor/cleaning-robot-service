using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;

namespace CleaningRobotService.Common.Robots;

public enum Plane
{
    Horizontal = 0,
    Vertical = 1,
}

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class RobotDictionaryLines : IRobot
{
    private int _count = 0;
    
    /// <summary>
    /// Second Key is the x or y coordinate.
    /// </summary>
    private readonly Dictionary<(Plane, int), List<Line>> _lines = new();
    
    public Point StartPoint { get; set; }
    public IEnumerable<CommandDto> Commands { get; set; } = Enumerable.Empty<CommandDto>();

    private struct Line
    {
        public Point Start;
        public Point End;
        public DirectionEnum Direction;
        
        private int GetLength()
        {
            return (int)Math.Sqrt(Math.Pow((End.X - Start.X), 2) + Math.Pow((End.Y - Start.Y), 2));
        }
        
        public List<Point> CalculatePoints()
        {
            Point currentPoint = Start;

            List<Point> points = new(GetLength() + 1) { currentPoint, };

            while (currentPoint != End)
            {
                switch (Direction)
                {
                    case DirectionEnum.north:
                        currentPoint.Y += 1;
                        break;
                    case DirectionEnum.east:
                        currentPoint.X += 1;
                        break;
                    case DirectionEnum.south:
                        currentPoint.Y -= 1;
                        break;
                    case DirectionEnum.west:
                        currentPoint.X -= 1;
                        break;
                    default:
                        throw new Exception("CommandDto direction of {command.Direction} not covered.");
                }

                points.Add(currentPoint);
            }

            return points;
        }
    }

    public void CalculatePointsVisited()
    {
        Point currentPoint = StartPoint;

        void AddPoint()
        {
            bool pointAlreadyOnLine = false;

            if (_lines.Any(x
                    => x.Key == (Plane.Horizontal, currentPoint.Y)
                    || x.Key == (Plane.Vertical, currentPoint.X)
                )
            )
            {
                pointAlreadyOnLine = _lines
                    .Any(x =>
                    {
                        if (x.Key == (Plane.Horizontal, currentPoint.Y))
                        {
                            List<Line> matchingLines = x.Value;
                            
                            return matchingLines
                                .Any(line => line.Start.X < line.End.X
                                    ? currentPoint.X >= line.Start.X && currentPoint.X <= line.End.X
                                    : currentPoint.X <= line.Start.X && currentPoint.X >= line.End.X
                                );
                        }
                        
                        if (x.Key == (Plane.Vertical, currentPoint.X))
                        {
                            List<Line> matchingLines = x.Value;
                            
                            return matchingLines.Any(line => line.Start.Y < line.End.Y
                                ? currentPoint.Y >= line.Start.Y && currentPoint.Y <= line.End.Y
                                : currentPoint.Y <= line.Start.Y && currentPoint.Y >= line.End.Y
                            );
                        }

                        return false;
                    });
            }

            // foreach (Line line in _lines)
            // {
            //     if (
            //         // Check the x coordinate is between the 2 lines.
            //         (line.Start.X < line.End.X
            //             ? currentPoint.X >= line.Start.X && currentPoint.X <= line.End.X
            //             : currentPoint.X <= line.Start.X && currentPoint.X >= line.End.X)
            //         // Check the y coordinate is between the 2 lines.
            //         && (line.Start.Y < line.End.Y
            //             ? currentPoint.Y >= line.Start.Y && currentPoint.Y <= line.End.Y
            //             : currentPoint.Y <= line.Start.Y && currentPoint.Y >= line.End.Y))
            //     {
            //         pointAlreadyOnLine = true;
            //         break;
            //     }
            // }

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
                Direction = command.Direction,
            };

            // TODO: move to different class
            {
                Plane plane = command.Direction is DirectionEnum.west or DirectionEnum.east
                    ? Plane.Horizontal
                    : Plane.Vertical;
                int key = plane == Plane.Horizontal
                    ? line.Start.Y
                    : line.Start.X;
            
                if (!_lines.ContainsKey((plane, key)))
                {
                    _lines
                        .Add((plane, key), new List<Line>());
                }
            
                _lines
                    .First(x => x.Key == (plane, key))
                    .Value
                    .Add(line);
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
