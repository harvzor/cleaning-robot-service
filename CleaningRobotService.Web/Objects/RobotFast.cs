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
    private readonly List<StartEnd> _startEnds = new();
    
    private class StartEnd
    {
        public Point StartPoint;
        public Point EndPoint;
    }

    public void CalculateStartEnds()
    {
        _currentPoint = StartPoint;

        foreach (var command in Commands.Where(command => command.Steps != 0))
        {
            Point start = _currentPoint;

            List<Point> pointsVisitedByThisCommand = new();
            switch (command.Direction)
            {
                case DirectionEnum.north:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        _currentPoint.Y += 1;
                        pointsVisitedByThisCommand.Add(_currentPoint);
                    }
                    break;
                case DirectionEnum.east:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        _currentPoint.X += 1;
                        pointsVisitedByThisCommand.Add(_currentPoint);
                    }
                    break;
                case DirectionEnum.south:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        _currentPoint.Y -= 1;
                        pointsVisitedByThisCommand.Add(_currentPoint);
                    }
                    break;
                case DirectionEnum.west:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        _currentPoint.X -= 1;
                        pointsVisitedByThisCommand.Add(_currentPoint);
                    }
                    break;
                default:
                    throw new ArgumentException(
                        message: $"Command direction of {command.Direction} not covered.",
                        paramName: nameof(Commands)
                    );
            }

            Point end = _currentPoint;

            List<Point> pointsWhichAreNotAlreadyThere = pointsVisitedByThisCommand
                .Where(point =>
                {
                    return _startEnds
                        .All(startEnd
                            => !Point.PointOnLine(startEnd.StartPoint, startEnd.EndPoint, point)
                        );
                })
                .ToList();
            
            if (pointsVisitedByThisCommand.Count == pointsWhichAreNotAlreadyThere.Count)
            {
                StartEnd startEnd = new()
                {
                    StartPoint = start,
                    EndPoint = end,
                };
                
                _startEnds.Add(startEnd);
            }
            else
            {
                foreach (Point point in pointsWhichAreNotAlreadyThere)
                {
                    StartEnd startEnd = new()
                    {
                        StartPoint = point,
                        EndPoint = point,
                    };
                
                    _startEnds.Add(startEnd);
                }
            }

            // if (pointsWhichAreNotAlreadyThere.Count == 0)
            // {
            //     return;
            // }
            //
            // if (pointsWhichAreNotAlreadyThere.Count == points.Count)
            // {
            //     StartEnd startEnd = new()
            //     {
            //         StartPoint = start,
            //         Direction = command.Direction,
            //         EndPoint = end,
            //     };
            //
            //     _startEnds.Add(startEnd);
            // }
            // else if (pointsWhichAreNotAlreadyThere.Count == 1)
            // {
            //     StartEnd startEnd = new()
            //     {
            //         StartPoint = pointsWhichAreNotAlreadyThere.First(),
            //         Direction = command.Direction,
            //         EndPoint = pointsWhichAreNotAlreadyThere.First(),
            //     };
            //
            //     _startEnds.Add(startEnd);
            // }
            // else
            // {
            //     foreach
            // }
        }
    }

    public int CalculateNumberOfPointsVisited()
    {
        CalculateStartEnds();

        return _startEnds
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