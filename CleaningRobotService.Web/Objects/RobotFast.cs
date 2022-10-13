using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Interfaces;
using CleaningRobotService.Web.Structs;

namespace CleaningRobotService.Web.Objects;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class RobotFast
{
    public Point StartPoint { get; set; }
    public IEnumerable<Command> Commands { get; set; } = Enumerable.Empty<Command>();
    private Point _currentPoint;
    private readonly List<Line> _lines = new();
    
    private class Line
    {
        public Point StartPoint;
        public DirectionEnum Direction;
        public Point EndPoint;
    }

    public void CalculateStartEnds()
    {
        _currentPoint = StartPoint;

        foreach (Command command in Commands.Where(command => command.Steps != 0))
        {
            Point start = _currentPoint;

            for (int i = 0; i < command.Steps; i++)
            {
                switch (command.Direction)
                {
                    case DirectionEnum.north:
                        _currentPoint.Y += 1;
                        break;
                    case DirectionEnum.east:
                        _currentPoint.X += 1;
                        break;
                    case DirectionEnum.south:
                        _currentPoint.Y -= 1;
                        break;
                    case DirectionEnum.west:
                        _currentPoint.X -= 1;
                        break;
                    default:
                        throw new ArgumentException(
                            message: $"Command direction of {command.Direction} not covered.",
                            paramName: nameof(Commands)
                        );
                }

                // If we're at the very start, this point can't already be counted for.
                if (start == StartPoint && i == 0)
                    continue;
                
                // If we're at the start of a new command, but not the first command, this point must already be counted for.
                if (i == 0)
                {
                    start = _currentPoint;
                    continue;
                }

                bool pointAlreadyAccountedFor = _lines
                    .Any(savedLine
                        => Point.PointOnLine(savedLine.StartPoint, savedLine.EndPoint, _currentPoint)
                    );
            
                // on each point step, check if it intersects another line
                //   if it does, close up the line and step again
                //   repeat (set start to null)
                if (pointAlreadyAccountedFor)
                {
                    if (start != _currentPoint)
                    {
                        _lines.Add(new Line()
                        {
                            StartPoint = start,
                            Direction = command.Direction,
                            EndPoint = _currentPoint,
                        });
                    }
                    
                    start = _currentPoint;
                }
            }

            _lines.Add(new Line()
            {
                StartPoint = start,
                Direction = command.Direction,
                EndPoint = _currentPoint,
            });
        }
    }

    public int CalculateNumberOfPointsVisited()
    {
        CalculateStartEnds();

        return _lines
           .Sum(startEnd
                   =>
               {
                   int distance = Point.GetDistance(startEnd.StartPoint, startEnd.EndPoint);

                   if (distance == 0)
                       return 1;

                   return distance;
               }
           )
           // Add one to include the start point.
           + 1;
    }
}