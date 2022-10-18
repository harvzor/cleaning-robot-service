using CleaningRobotService.Common.Helpers;
using CleaningRobotService.Common.Interfaces;
using CleaningRobotService.Common.Objects;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Mappers;
using CleaningRobotService.Web.Models;

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

        // IRobot robot = new RobotPoints
        IRobot robot = new RobotLines
        // IRobot robot = new RobotGrid
        // IRobot robot = new RobotSwarm
        {
            StartPoint = body.Start,
            Commands = body.Commands.ToCommonCommandDtos(),
        };
        
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