using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Interfaces;
using CleaningRobotService.Web.Structs;

namespace CleaningRobotService.Web.Objects;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class RobotRough
{
    public Point StartPoint { get; set; }
    public IEnumerable<Command> Commands { get; set; } = Enumerable.Empty<Command>();
    private Point _currentPoint;
    private readonly List<StartEnd> _startEnds = new();
    
    private class StartEnd
    {
        public Point StartPoint;
        public Point EndPoint;
        public DirectionEnum Direction;
    }

    public void CalculateStartEnds()
    {
        _currentPoint = StartPoint;

        foreach (var command in Commands.Where(command => command.Steps != 0))
        {
            Point start = _currentPoint;
            
            switch (command.Direction)
            {
                case DirectionEnum.north:
                    _currentPoint.Y += (int)command.Steps;
                    break;
                case DirectionEnum.east:
                    _currentPoint.X += (int)command.Steps;
                    break;
                case DirectionEnum.south:
                    _currentPoint.Y -= (int)command.Steps;
                    break;
                case DirectionEnum.west:
                    _currentPoint.X -= (int)command.Steps;
                    break;
                default:
                    throw new ArgumentException(
                        message: $"Command direction of {command.Direction} not covered.",
                        paramName: nameof(Commands)
                    );
            }

            Point end = _currentPoint;

            bool shouldBeSwapped = command.Direction is DirectionEnum.east or DirectionEnum.west
                ? Point.IsMoreEast(start, end)
                : Point.IsMoreNorth(start, end);
            
            // Normalise new StartEnd so the EndPoint is more positive than the StartPoint.
            StartEnd startEnd = new()
            {
                StartPoint = shouldBeSwapped ? end : start,
                Direction = command.Direction,
                EndPoint = shouldBeSwapped ? start : end,
            };

            bool replaced = false;
            
            // Check for any _startEnds where this new startEnd starts or ends between.
            // TODO: must be a way to make code in statement into method (DRY it up).
            if (command.Direction is DirectionEnum.east or DirectionEnum.west)
            {
                StartEnd? matchingStartEndIfStartOrEndIsOnLine = _startEnds
                    // Where the Y coordinate is the same.
                    .Where(x => x.StartPoint.Y == startEnd.StartPoint.Y)
                    // And the new startEnd starts or ends along any other in _startEnds.
                    .Where(x
                        => Point.PointOnLine(pt1: x.StartPoint, pt2: x.EndPoint, pt: startEnd.StartPoint)
                           || Point.PointOnLine(pt1: x.StartPoint, pt2: x.EndPoint, pt: startEnd.EndPoint)
                    )
                    .FirstOrDefault();

                if (matchingStartEndIfStartOrEndIsOnLine != null)
                {
                    replaced = true;
                    // If the new StartPoint is more west than the old StartPoint, swap them.
                    if (Point.IsMoreWest(p1: matchingStartEndIfStartOrEndIsOnLine.StartPoint, p2: startEnd.StartPoint))
                        matchingStartEndIfStartOrEndIsOnLine.StartPoint = startEnd.StartPoint;
                    
                    // If the new EndPoint is more east than the old EndPoint, swap them.
                    if (Point.IsMoreEast(p1: matchingStartEndIfStartOrEndIsOnLine.EndPoint, p2: startEnd.EndPoint))
                        matchingStartEndIfStartOrEndIsOnLine.EndPoint = startEnd.EndPoint;
                }
            }
            else
            {
                StartEnd? matchingStartEndIfStartOrEndIsOnLine = _startEnds
                    // Where the Y coordinate is the same.
                    .Where(x => x.StartPoint.X == startEnd.StartPoint.X)
                    // And the new startEnd starts or ends along any other in _startEnds.
                    .Where(x
                        => Point.PointOnLine(pt1: x.StartPoint, pt2: x.EndPoint, pt: startEnd.StartPoint)
                        || Point.PointOnLine(pt1: x.StartPoint, pt2: x.EndPoint, pt: startEnd.EndPoint)
                    )
                    .FirstOrDefault();

                if (matchingStartEndIfStartOrEndIsOnLine != null)
                {
                    replaced = true;
                    // If the new StartPoint is more north than the old StartPoint, swap them.
                    if (Point.IsMoreNorth(p1: matchingStartEndIfStartOrEndIsOnLine.StartPoint, p2: startEnd.StartPoint))
                        matchingStartEndIfStartOrEndIsOnLine.StartPoint = startEnd.StartPoint;
                    
                    // If the new EndPoint is more south than the old EndPoint, swap them.
                    if (Point.IsMoreSouth(p1: matchingStartEndIfStartOrEndIsOnLine.EndPoint, p2: startEnd.EndPoint))
                        matchingStartEndIfStartOrEndIsOnLine.EndPoint = startEnd.EndPoint;
                }
            }
            
            // If the new startEnd didn't overlap any other startEnds, then just add it.
            if (!replaced)
                _startEnds.Add(startEnd);
        }
    }

    public int CalculateNumberOfPointsVisited()
    {
        CalculateStartEnds();

        return _startEnds
            .Sum(startEnd => Point.GetDistance(startEnd.StartPoint, startEnd.EndPoint))
            // Add one because the distance between 2 points doesn't include the initial point.
            + 1;
    }
}