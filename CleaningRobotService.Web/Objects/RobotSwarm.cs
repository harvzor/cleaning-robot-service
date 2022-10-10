using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Interfaces;
using CleaningRobotService.Web.Structs;

namespace CleaningRobotService.Web.Objects;

/// <summary>
/// Simulate lots of robots cleaning an office.
/// </summary>
/// <remarks>
/// We can just throw more cooks in the kitchen, right?
/// </remarks>
public class RobotSwarm : IRobot
{
    public Point StartPoint { get; set; }
    public IEnumerable<Command> Commands { get; set; } = Enumerable.Empty<Command>();
    private readonly int _chunkCommands = 1000;

    public RobotSwarm(int chunkCommands = 1000)
    {
        _chunkCommands = chunkCommands;
    }

    public IEnumerable<Point> CalculatePointsVisited()
    {
        IEnumerable<Command[]> commandChunks = Commands
            .Chunk(_chunkCommands);

        Dictionary<long, IEnumerable<Point>> results = new(commandChunks.Count());
        
        Parallel.ForEach(commandChunks, (commands, _, index) =>
        {
            Robot robot = new()
            {
                StartPoint = new Point
                {
                    X = 0,
                    Y = 0,
                },
                Commands = commands,
            };

            results.Add(index, robot.CalculatePointsVisited());
        });
        
        HashSet<Point> pointsVisited = new() { StartPoint, };

        Point lastPosition = StartPoint;
        foreach (IEnumerable<Point> points in results
             .OrderBy(x => x.Key)
             .Select(x => x.Value))
        {
            foreach (Point point in points)
            {
                Point translatedPoint = point.Combine(lastPosition);

                if (!pointsVisited.Contains(translatedPoint))
                    pointsVisited.Add(translatedPoint);
            }

            lastPosition = points.Last().Combine(lastPosition);
        }

        return pointsVisited;
    }
}
