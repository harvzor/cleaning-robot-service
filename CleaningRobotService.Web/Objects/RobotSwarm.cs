using System.Drawing;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Interfaces;

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
    
    public RobotSwarm()
    {
    }

    public RobotSwarm(int chunkCommands = 1000)
    {
        _chunkCommands = chunkCommands;
    }

    public IEnumerable<Point> CalculatePointsVisited()
    {
        List<Command[]> commandChunks = Commands
            .Chunk(_chunkCommands)
            .ToList();

        List<Point>[] results = new List<Point>[commandChunks.Count];
        
        Parallel.ForEach(commandChunks, (commands, _, index) =>
        {
            RobotPoints robotPoints = new()
            {
                StartPoint = new Point
                {
                    X = 0,
                    Y = 0,
                },
                Commands = commands,
            };

            results[index] = robotPoints.CalculatePointsVisited().ToList();
        });
        
        HashSet<Point> pointsVisited = new() { StartPoint, };

        Point lastPosition = StartPoint;
        foreach (List<Point> points in results)
        {
            foreach (Point point in points)
            {
                point.Offset(lastPosition);

                if (!pointsVisited.Contains(point))
                    pointsVisited.Add(point);
            }

            lastPosition = points.Last();
        }

        return pointsVisited;
    }
}
