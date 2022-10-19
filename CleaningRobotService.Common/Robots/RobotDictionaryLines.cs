using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;

namespace CleaningRobotService.Common.Robots;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class RobotDictionaryLines : IRobot
{
    private int _count = 0;
    
    /// <summary>
    /// Key is Y coordinate. 
    /// </summary>
    private readonly Dictionary<int, List<Line>> _horizontalLines = new();
    /// <summary>
    /// Key is X coordinate. 
    /// </summary>
    private readonly Dictionary<int, List<Line>> _verticalLines = new();
    
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

            if (_horizontalLines.Any(x => x.Key == currentPoint.Y))
            {
                List<Line> matchingHorizontalLines = _horizontalLines
                    .First(x => x.Key == currentPoint.Y)
                    .Value;

                pointAlreadyOnLine = matchingHorizontalLines
                    .Any(line => line.Start.X < line.End.X
                        ? currentPoint.X >= line.Start.X && currentPoint.X <= line.End.X
                        : currentPoint.X <= line.Start.X && currentPoint.X >= line.End.X
                    );
            }
            
            if (!pointAlreadyOnLine && _verticalLines.Any(x => x.Key == currentPoint.X))
            {
                List<Line> matchingHorizontalLines = _verticalLines
                    .First(x => x.Key == currentPoint.X)
                    .Value;

                pointAlreadyOnLine = matchingHorizontalLines
                    .Any(line => line.Start.Y < line.End.Y
                        ? currentPoint.Y >= line.Start.Y && currentPoint.Y <= line.End.Y
                        : currentPoint.Y <= line.Start.Y && currentPoint.Y >= line.End.Y
                    );
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

            if (command.Direction is DirectionEnum.west or DirectionEnum.east)
            {
                int key = line.Start.Y;
                if (!_horizontalLines.ContainsKey(key))
                {
                    _horizontalLines
                        .Add(key, new List<Line>());
                }
                
                _horizontalLines
                    .First(x => x.Key == key)
                    .Value
                    .Add(line);
            }
            else
            {
                int key = line.Start.X;
                if (!_verticalLines.ContainsKey(key))
                {
                    _verticalLines
                        .Add(key, new List<Line>());
                }
                
                _verticalLines
                    .First(x => x.Key == key)
                    .Value
                    .Add(line);
            }
        }
    }

    public IEnumerable<Point> GetPointsVisited()
    {
        List<Point> horizontalLinePoints = _horizontalLines
            .SelectMany(group => group.Value)
            .SelectMany(line => line.CalculatePoints())
            .Distinct()
            .ToList();
        
        List<Point> verticalLinePoints = _verticalLines
            .SelectMany(group => group.Value)
            .SelectMany(line => line.CalculatePoints())
            .Distinct()
            .ToList();

        return horizontalLinePoints.Union(verticalLinePoints);
    }

    public int CountPointsVisited() => _count;
}
