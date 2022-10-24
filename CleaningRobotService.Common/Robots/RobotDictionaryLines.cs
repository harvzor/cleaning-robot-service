using System.Collections;
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
public class RobotDictionaryLines : IRobot
{
    private LineDictionary? _store;
    public Point StartPoint { get; set; }
    public IEnumerable<CommandDto> Commands { get; set; } = Enumerable.Empty<CommandDto>();

    public void CalculatePointsVisited()
    {
        _store = new LineDictionary(numberOfExpectedCommands: Commands.Count());

        Point currentPoint = StartPoint;

        void AddPoint()
        {
            _store.AddPoint(point: currentPoint);
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

            Line line = new Line(start: start, end: currentPoint);
            
            _store.AddLine(line: line);
        }
    }

    public IEnumerable<Point> GetPointsVisited() => _store.GetPoints();

    public int CountPointsVisited() => _store.Count();
}
