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
    
    public int CalculateIndicesCleaned(Point startPoint, List<Command> commands)
    {
        Point currentPoint = startPoint;

        HashSet<Point> pointsVisited = new() { currentPoint, };
        
        void Step(uint steps, Action action)
        {
            for (int i = 0; i < steps; i++)
            {
                action();
                        
                if (!pointsVisited.Contains(currentPoint))
                    pointsVisited.Add(currentPoint);
            }
        }

        foreach (Command command in commands)
        {
            if (command.Steps == 0)
                continue;
            
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
                        paramName: nameof(commands)
                    );
            }
        }

        return pointsVisited.Count;
    }

    public Execution CreateCommandRobot(CommandRobotPostDto body)
    {
        DateTimeOffset now = SystemDateTime.UtcNow;
        int? result = null;
        
        float calculationTime = MethodTimer.Measure(() =>
        {
            result = this.CalculateIndicesCleaned(startPoint: body.Start, commands: body.Commands);
        });

        Execution execution = new()
        {
            TimeStamp = now,
            Commands = body.Commands.Count,
            Result = result!.Value,
            Duration = calculationTime,
        };
        
        base.Context.Executions.Add(execution);

        base.Context.SaveChanges();

        return execution;
    }
}