using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;

namespace CleaningRobotService.Common.Robots;

/// <summary>
/// Simulate lots of robots cleaning an office.
/// </summary>
/// <remarks>
/// We can just throw more cooks in the kitchen, right?
/// </remarks>
public class RobotSwarm : IRobot
{
    public Point StartPoint { get; set; }
    public IEnumerable<CommandDto> Commands { get; set; } = Enumerable.Empty<CommandDto>();
    
    private int _count = 0;
    private readonly int _chunkCommands = 1000;
    private readonly HashSet<Point> _pointsVisited = new();
    
    public RobotSwarm()
    {
    }

    public RobotSwarm(int chunkCommands = 1000)
    {
        _chunkCommands = chunkCommands;
    }

    public void CalculatePointsVisited()
    {
        _pointsVisited.Add(StartPoint);
        
        List<CommandDto[]> commandChunks = Commands
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

            results[index] = robotPoints.GetPointsVisited().ToList();
        });

        Point lastPosition = StartPoint;
        foreach (List<Point> points in results)
        {
            foreach (Point point in points)
            {
                point.Offset(lastPosition);

                if (_pointsVisited.Add(point))
                    _count++;
            }

            lastPosition = points.Last();
        }
    }

    public IEnumerable<Point> GetPointsVisited() => _pointsVisited;

    public int CountPointsVisited() => _count;
}
