using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Structs;

namespace CleaningRobotService.Web.Objects;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class Robot
{
    private Point _currentPoint;

    public Robot(Point startPoint)
    {
        _currentPoint = startPoint;
    }
    
    public int CalculateIndicesVisited(List<Command> commands)
    {
        HashSet<Point> pointsVisited = new() { _currentPoint, };
        
        void Step(uint steps, Action action)
        {
            for (int i = 0; i < steps; i++)
            {
                action();
                        
                if (!pointsVisited.Contains(_currentPoint))
                    pointsVisited.Add(_currentPoint);
            }
        }

        // For loops are supposed to be faster but I'm not gonna sacrifice readability for performance.
        foreach (var command in commands.Where(command => command.Steps != 0))
        {
            switch (command.Direction)
            {
                case DirectionEnum.north:
                    Step(steps: command.Steps, action: () => _currentPoint.Y++);
                    break;
                case DirectionEnum.east:
                    Step(steps: command.Steps, action: () => _currentPoint.X++);
                    break;
                case DirectionEnum.south:
                    Step(steps: command.Steps, action: () => _currentPoint.Y--);
                    break;
                case DirectionEnum.west:
                    Step(steps: command.Steps, action: () => _currentPoint.X--);
                    break;
                default:
                    throw new ArgumentException(
                        message: $"Command direction of {command.Direction} not covered.",
                        paramName: nameof(commands)
                    );
            }
        }

        return pointsVisited.Count;
    }
}