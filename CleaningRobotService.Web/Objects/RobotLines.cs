using System.Drawing;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Interfaces;

namespace CleaningRobotService.Web.Objects;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class RobotLines : IRobot
{
    private int _count = 0;
    private List<Line> _lines = new();
    
    public Point StartPoint { get; set; }
    public IEnumerable<Command> Commands { get; set; } = Enumerable.Empty<Command>();

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
                        throw new Exception("Command direction of {command.Direction} not covered.");
                }

                points.Add(currentPoint);
            }

            return points;
        }
    }

    public void CalculatePointsVisited()
    {
        _lines = new List<Line>(Commands.Count());
        
        Point currentPoint = StartPoint;

        void AddPoint()
        {
            bool pointAlreadyOnLine = false;
            foreach (var line in _lines)
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

        foreach (Command command in Commands.Where(command => command.Steps != 0))
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
                            message: $"Command direction of {command.Direction} not covered.",
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

            _lines.Add(line);
        }
    }

    public IEnumerable<Point> GetPointsVisited() => _lines
        .SelectMany(line => line.CalculatePoints())
        .Distinct();

    public int CountPointsVisited() => _count;
}
