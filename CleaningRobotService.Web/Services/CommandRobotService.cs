using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Helpers;
using CleaningRobotService.Web.Interfaces;
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

        // IRobot robot = new RobotPoints
        IRobot robot = new RobotLines
        // IRobot robot = new RobotGrid
        // IRobot robot = new RobotSwarm
        {
            StartPoint = body.Start,
            Commands = body.Commands,
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