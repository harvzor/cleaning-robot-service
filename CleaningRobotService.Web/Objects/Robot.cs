using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Interfaces;
using CleaningRobotService.Web.Structs;

namespace CleaningRobotService.Web.Objects;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class Robot : IRobot
{
    public Point StartPoint { get; set; }
    public IEnumerable<Command> Commands { get; set; } = Enumerable.Empty<Command>();
    private Point _currentPoint;
    private readonly List<Result> _cachedResults = new();
    private readonly HashSet<Point> _pointsVisited = new();
    
    private class Result
    {
        public Point StartPoint;
        public Command? Command;
        public Point EndPoint;
    }
    
    private void CalculateCommand(Command command)
    {
        List<Point> potentiallyNewPointsVisited = Enumerable.Range(0, (int)command.Steps)
            .Select(_ =>
            {
                switch (command.Direction)
                {
                    case DirectionEnum.north:
                        _currentPoint.Y++;
                        break;
                    case DirectionEnum.east:
                        _currentPoint.X++;
                        break;
                    case DirectionEnum.south:
                        _currentPoint.Y--;
                        break;
                    case DirectionEnum.west:
                        _currentPoint.X--;
                        break;
                    default:
                        throw new ArgumentException(
                            message: $"Command direction of {command.Direction} not covered.",
                            paramName: nameof(Commands)
                        );
                }

                return _currentPoint;
            })
            .ToList();
        
        _pointsVisited.UnionWith(potentiallyNewPointsVisited);
        
        // for (int i = 0; i < steps; i++)
        // {
        //     action();
        //
        //     if (!_pointsVisited.Contains(_currentPoint))
        //         _pointsVisited.Add(_currentPoint);
        // }
    }

    private void CalculatePointVisited(Command command)
    {
        Point startPoint = _currentPoint;
        
        Result? cachedResult = _cachedResults.FirstOrDefault(x
            => x.StartPoint == _currentPoint 
            && x.Command.Direction == command.Direction
            && x.Command.Steps == command.Steps
        );

        if (cachedResult != null)
        {
            _currentPoint = cachedResult.EndPoint;
            return;
        }

        CalculateCommand(command: command);
        
        _cachedResults.Add(new Result
        {
            StartPoint = startPoint,
            Command = command,
            EndPoint = _currentPoint,
        });
    }

    public IEnumerable<Point> CalculatePointsVisited()
    {
        _pointsVisited.Add(StartPoint);

        _currentPoint = StartPoint;

        foreach (var command in Commands.Where(command => command.Steps != 0))
        {
            CalculatePointVisited(command: command);
        }

        return _pointsVisited;
    }
}