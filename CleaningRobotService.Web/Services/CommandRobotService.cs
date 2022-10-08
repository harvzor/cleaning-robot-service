using System.Diagnostics;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Helpers;
using CleaningRobotService.Web.Models;
using CleaningRobotService.Web.Structs;

namespace CleaningRobotService.Web.Services;

public class CommandRobotService : BaseService
{
    public CommandRobotService(ServiceDbContext context) : base(context)
    {
    }
    
    public int CalculateIndicesCleaned(Point startPoint, List<Command> commands, out float calculationTime)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();
        
        Point currentPoint = startPoint;

        HashSet<Point> pointsVisited = new() { currentPoint, };

        foreach (Command command in commands)
        {
            switch (command.Direction)
            {
                case DirectionEnum.north:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        currentPoint.Y++;
                        
                        if (!pointsVisited.Contains(currentPoint))
                            pointsVisited.Add(currentPoint);
                    }
                    
                    break;
                case DirectionEnum.east:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        currentPoint.X++;
                        
                        if (!pointsVisited.Contains(currentPoint))
                            pointsVisited.Add(currentPoint);
                    }
                    
                    break;
                case DirectionEnum.south:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        currentPoint.Y--;
                        
                        if (!pointsVisited.Contains(currentPoint))
                            pointsVisited.Add(currentPoint);
                    }
                    
                    break;
                case DirectionEnum.west:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        currentPoint.X--;
                        
                        if (!pointsVisited.Contains(currentPoint))
                            pointsVisited.Add(currentPoint);
                    }
                    
                    break;
                default:
                    // TODO: replace with a proper exception (don't use Exception!)
                    throw new Exception("Command direction case not covered.");
            }
        }
        
        stopWatch.Stop();
        calculationTime = stopWatch.ElapsedMilliseconds * 1000;

        return pointsVisited.Count;
    }

    public Execution CreateCommandRobot(CommandRobotPostDto body)
    {
        DateTimeOffset now = SystemDateTime.UtcNow;
        
        int result = this.CalculateIndicesCleaned(startPoint: body.Start, commands: body.Commands, out float calculationTime);

        Execution execution = new()
        {
            TimeStamp = now,
            Commands = body.Commands.Count,
            Result = result,
            Duration = calculationTime,
        };
        
        base.Context.Executions.Add(execution);

        base.Context.SaveChanges();

        return execution;
    }
}