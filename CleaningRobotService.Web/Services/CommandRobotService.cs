using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Helpers;
using CleaningRobotService.Web.Models;
using CleaningRobotService.Web.Objects;

namespace CleaningRobotService.Web.Services;

public class CommandRobotService : BaseService
{
    public CommandRobotService(ServiceDbContext context) : base(context)
    {
    }

    public Execution CreateCommandRobot(CommandRobotPostDto body)
    {
        DateTimeOffset now = SystemDateTime.UtcNow;
        int? result = null;

        Robot robot = new(startPoint: body.Start);
        
        double calculationTime = MethodTimer.Measure(() =>
        {
            result = robot.CalculateIndicesVisited(commands: body.Commands);
        });

        Execution execution = new()
        {
            TimeStamp = now,
            Commands = body.Commands.Count,
            Result = result!.Value,
            Duration = calculationTime,
        };
        
        Context.Executions.Add(execution);

        Context.SaveChanges();

        return execution;
    }
}