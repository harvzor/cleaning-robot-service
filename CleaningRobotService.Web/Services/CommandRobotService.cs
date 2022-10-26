using CleaningRobotService.Common.Factories;
using CleaningRobotService.Common.Helpers;
using CleaningRobotService.Common.Robots;
using CleaningRobotService.DataPersistence;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Mappers;

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
        
        IRobot robot = new RobotFactory()
            .GetRobot(startPoint: body.Start, commands: body.Commands.ToCommonCommandDtos());
        
        double calculationTime = MethodTimer.Measure(() =>
        {
            robot.CalculatePointsVisited();
            result = robot.CountPointsVisited();
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