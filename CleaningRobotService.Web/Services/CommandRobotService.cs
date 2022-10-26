using CleaningRobotService.Common.Factories;
using CleaningRobotService.Common.Helpers;
using CleaningRobotService.Common.Robots;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.DataPersistence.Repositories;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Mappers;

namespace CleaningRobotService.Web.Services;

public class CommandRobotService
{
    private readonly IExecutionRepository _repository;
    
    public CommandRobotService(IExecutionRepository repository)
    {
        _repository = repository;
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

        _repository.Add(execution);

        _repository.Save();

        return execution;
    }
}