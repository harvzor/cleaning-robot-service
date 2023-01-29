using CleaningRobotService.BusinessLogic.Mappers;
using CleaningRobotService.Common.Dtos;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Factories;
using CleaningRobotService.Common.Helpers;
using CleaningRobotService.Common.Robots;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.DataPersistence.Repositories;
using DirectionStep = CleaningRobotService.Common.Dtos.Input.DirectionStep;

namespace CleaningRobotService.BusinessLogic.Services;

public class CommandRobotService : ICommandRobotService
{
    private readonly IExecutionRepository _executionRepository;
    private readonly ICommandRepository _commandRepository;

    public CommandRobotService(ICommandRepository commandRepository, IExecutionRepository executionRepository)
    {
        _executionRepository = executionRepository;
        _commandRepository = commandRepository;
    }

    public void RunExecution(Guid executionId)
    {
        int? result = null;
        
        Execution? execution = _executionRepository
            .QueryObjectGraph(
                filter: x => x.Id == executionId,
                includeChildren: x => x.Command
            )
            .FirstOrDefault();

        if (execution == null)
            throw new ArgumentException("ID refers to missing database item.", nameof(executionId));

        IRobot robot = new RobotFactory()
            .GetRobot(startPoint: execution.Command.StartPoint, commands: execution.Command.DirectionSteps.ToDtos());

        TimeSpan calculationTime = MethodTimer.Measure(() =>
        {
            robot.CalculatePointsVisited();
            result = robot.CountPointsVisited();
        });

        execution.Result = result;
        execution.Duration = calculationTime;
        
        _executionRepository.Save();
    }

    public Command CreateCommandRobot(
        PointDto startPoint,
        IReadOnlyCollection<DirectionStep> commands,
        bool runExecutionAsync = true
    )
    {
        DateTimeOffset now = SystemDateTime.UtcNow;
        
        Command command = new Command
        {
            StartPoint = startPoint.ToModel(),
            DirectionSteps = commands.ToModels().ToList(),
            CreatedAt = now,
            ModifiedAt = now,
        };
        
        _commandRepository.Add(command);
        
        _commandRepository.Save();

        Execution execution = new()
        {
            CreatedAt = now,
            ModifiedAt = now,
            Result = null,
            Duration = null,
            CommandId = command.Id,
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
        
        return command;
    }
}
