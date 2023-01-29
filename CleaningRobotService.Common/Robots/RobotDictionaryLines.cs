using System.Drawing;
using CleaningRobotService.Common.Collections;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Structures;

namespace CleaningRobotService.Common.Robots;

/// <summary>
/// This robot works best when many commands are being sent and each command has a lot of steps (>5).
/// Memory usage only scales by number of commands.
/// </summary>
public class RobotDictionaryLines : BaseRobot, IRobot
{
    private readonly LineDictionary _store;

    public RobotDictionaryLines(Point startPoint, IEnumerable<DirectionStep> commands)
        : base(startPoint: startPoint, commands: commands)
    {
        _store = new LineDictionary(numberOfExpectedCommands: Commands.Count);
    }

    public void CalculatePointsVisited()
    {
        Point currentPoint = StartPoint;

        void AddPoint()
        {
            _store.AddPoint(point: currentPoint);
        }
        
        AddPoint();

        foreach (DirectionStep command in Commands.Where(command => command.Steps != 0))
        {
            Point start = currentPoint;
            
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

            Line line = new Line(start: start, end: currentPoint);
            
            _store.AddLine(line: line);
        }
    }

    public IEnumerable<Point> GetPointsVisited() => _store.GetPoints();

    public int CountPointsVisited() => _store.Count();
}
