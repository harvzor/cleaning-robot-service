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
            
            // Normalise new StartEnd so the end point is more positive than the start.
            // Check for any _startEnds where this new StartEnd starts or ends between.
            // If there are any, replace its start and end with the new start or end, if they are greater.
            
            _startEnds.Add(new StartEnd
            {
                StartPoint = start,
                Direction = command.Direction,
                EndPoint = _currentPoint,
            });
        }
    }

    public int CalculatePointsVisited()
    {
        CalculateStartEnds();

        // Remove overlapping points.

        List<StartEnd> xGroup = _startEnds
            .Where(startEnd => startEnd.Direction is DirectionEnum.east or DirectionEnum.west)
            .ToList();
        
        List<StartEnd> yGroup = _startEnds
            .Where(startEnd => startEnd.Direction is DirectionEnum.north or DirectionEnum.south)
            .ToList();
        
        // Where Y values are the same.
        List<IGrouping<int, StartEnd>> xGroupGroupedBySameY = xGroup
            .GroupBy(x => x.StartPoint.Y)
            .ToList();
        
        // Simplify overlapping into less paths.
        List<StartEnd> xGroupSimplified = new();
        foreach (IGrouping<int, StartEnd> group in xGroupGroupedBySameY)
        {
            
        }
        
        return _startEnds
            .Sum(startEnd => Point.GetDistance(startEnd.StartPoint, startEnd.EndPoint));
    }
}