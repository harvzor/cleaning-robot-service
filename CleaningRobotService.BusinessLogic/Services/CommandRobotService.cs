using System.Drawing;
using CleaningRobotService.BusinessLogic.Mappers;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Factories;
using CleaningRobotService.Common.Helpers;
using CleaningRobotService.Common.Robots;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.DataPersistence.Repositories;

namespace CleaningRobotService.BusinessLogic.Services;

public class CommandRobotService : ICommandRobotService
{
    private readonly IExecutionRepository _executionRepository;
    private readonly ICommandRobotRepository _commandRobotRepository;

    public CommandRobotService(ICommandRobotRepository commandRobotRepository, IExecutionRepository executionRepository)
    {
        _executionRepository = executionRepository;
        _commandRobotRepository = commandRobotRepository;
    }

    public void RunExecution(Guid executionId)
    {
        int? result = null;
        
        Execution? execution = _executionRepository
            .QueryObjectGraph(
                filter: x => x.Id == executionId,
                includeChildren: x => x.CommandRobot
            )
            .FirstOrDefault();

        if (execution == null)
            throw new ArgumentException("ID refers to missing database item.", nameof(executionId));

        IRobot robot = new RobotFactory()
            .GetRobot(startPoint: execution.CommandRobot.StartPoint, commands: execution.CommandRobot.Commands.ToDtos());

        TimeSpan calculationTime = MethodTimer.Measure(() =>
        {
            robot.CalculatePointsVisited();
            result = robot.CountPointsVisited();
        });

        execution.Result = result;
        execution.Duration = calculationTime;
        
        _executionRepository.Save();
    }

    public CommandRobot CreateCommandRobot(
        Point startPoint,
        IReadOnlyCollection<CommandDto> commands,
        bool runExecutionAsync = true
    )
    {
        CommandRobot commandRobot = new CommandRobot
        {
            StartPoint = startPoint,
            Commands = commands.ToModels().ToList(),
        };

        DateTimeOffset now = SystemDateTime.UtcNow;
        
        _commandRobotRepository.Add(commandRobot);

        Execution execution = new()
        {
            CreatedAt = now,
            ModifiedAt = now,
            Result = null,
            Duration = null,
            CommandRobotId = commandRobot.Id,
            Commands = commands.Count,
            // Result = result!.Value,
            // Duration = calculationTime,
        };

        _executionRepository.Add(execution);
        
        _executionRepository.Save();

        if (runExecutionAsync)
            // Run on a new thread as it might take some time.
            Task.Run(() => RunExecution(executionId: execution.Id));
        else
            RunExecution(executionId: execution.Id);
        
        return commandRobot;
    }
}
