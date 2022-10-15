using System.Drawing;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Interfaces;

namespace CleaningRobotService.Web.Objects;

/// <summary>
/// Simulated robot which cleans the office.
/// </summary>
public class Robot2DArray : IRobot
{
    public Point StartPoint { get; set; }
    public IEnumerable<Command> Commands { get; set; } = Enumerable.Empty<Command>();

    public IEnumerable<Point> CalculatePointsVisited()
    {
        Point currentPoint = StartPoint;
        // Will allocate ~5GB of data?
        // const int squareWidth = 200000; // Array dimensions exceeded supported range
        const int squareWidth = 500;
        bool[,] pointsVisited = new bool[squareWidth, squareWidth];

        pointsVisited[squareWidth / 2 + StartPoint.X, squareWidth / 2 + StartPoint.Y] = true;

        void Step(uint steps, Action action)
        {
            for (int i = 0; i < steps; i++)
            {
                action();

                pointsVisited[squareWidth / 2 + currentPoint.X, squareWidth / 2 + currentPoint.Y] = true;
            }
        }

        // For loops are supposed to be faster but I'm not gonna sacrifice readability for performance.
        foreach (var command in Commands.Where(command => command.Steps != 0))
        {
            switch (command.Direction)
            {
                case DirectionEnum.north:
                    Step(steps: command.Steps, action: () => currentPoint.Y++);
                    break;
                case DirectionEnum.east:
                    Step(steps: command.Steps, action: () => currentPoint.X++);
                    break;
                case DirectionEnum.south:
                    Step(steps: command.Steps, action: () => currentPoint.Y--);
                    break;
                case DirectionEnum.west:
                    Step(steps: command.Steps, action: () => currentPoint.X--);
                    break;
                default:
                    throw new ArgumentException(
                        message: $"Command direction of {command.Direction} not covered.",
                        paramName: nameof(Commands)
                    );
            }
        }

        for (int x = 0; x < squareWidth; x++)
        {
            for (int y = 0; y < squareWidth; y++)
            {
                if (pointsVisited[x, y])
                {
                    yield return new Point
                    {
                        X = x - squareWidth / 2,
                        Y = y - squareWidth / 2,
                    };
                }
            }
        }
    }
}