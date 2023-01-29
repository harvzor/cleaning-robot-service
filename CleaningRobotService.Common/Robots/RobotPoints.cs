using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;

namespace CleaningRobotService.Common.Robots;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class RobotPoints : BaseRobot, IRobot
{
    private int _count;
    // https://stackoverflow.com/questions/24855615/hashset-memory-overhead
    private readonly HashSet<Point> _pointsVisited = new();

    public RobotPoints(Point startPoint, IEnumerable<DirectionStep> commands)
        : base(startPoint: startPoint, commands: commands)
    {
    }

    public void CalculatePointsVisited()
    {
        Point currentPoint = StartPoint;

        void AddPoint()
        {
            if (_pointsVisited.Add(currentPoint))
                _count++;
        }
        
        AddPoint();

        foreach (DirectionStep command in Commands.Where(command => command.Steps != 0))
        {
            for (int i = 0; i < command.Steps; i++)
            {
                switch (command.Direction)
                {
                    case DirectionEnum.North:
                        currentPoint.Y++;
                        AddPoint();
                        break;
                    case DirectionEnum.East:
                        currentPoint.X++;
                        AddPoint();
                        break;
                    case DirectionEnum.South:
                        currentPoint.Y--;
                        AddPoint();
                        break;
                    case DirectionEnum.West:
                        currentPoint.X--;
                        AddPoint();
                        break;
                    default:
                        throw new ArgumentException(
                            message: $"DirectionStep direction of {command.Direction} not covered.",
                            paramName: nameof(Commands)
                        );
                }
            }
        }
    }

    public IEnumerable<Point> GetPointsVisited() =>  _pointsVisited;

    public int CountPointsVisited() => _count;
}
