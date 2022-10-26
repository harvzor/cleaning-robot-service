using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Factories;
using CleaningRobotService.Common.Helpers;
using CleaningRobotService.Common.Robots;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.DataPersistence.Repositories;

namespace CleaningRobotService.Common.Services;

public class CommandRobotService : ICommandRobotService
{
    private readonly IExecutionRepository _repository;
    
    public CommandRobotService(IExecutionRepository repository)
    {
        _repository = repository;
    }

    public Execution CreateCommandRobot(Point startPoint, IReadOnlyCollection<CommandDto> commands)
    {
        DateTimeOffset now = SystemDateTime.UtcNow;
        int? result = null;
        
        IRobot robot = new RobotFactory()
            .GetRobot(startPoint: startPoint, commands: commands);
        
        double calculationTime = MethodTimer.Measure(() =>
        {
            robot.CalculatePointsVisited();
            result = robot.CountPointsVisited();
        });

        Execution execution = new()
        {
            TimeStamp = now,
            Commands = commands.Count,
            Result = result!.Value,
            Duration = calculationTime,
        };

        _repository.Add(execution);

        _repository.Save();

        return execution;
    }
}