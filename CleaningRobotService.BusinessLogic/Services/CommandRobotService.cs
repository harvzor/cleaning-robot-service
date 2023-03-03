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
    private readonly ICommandRepository _commandRepository;

    public CommandRobotService(ICommandRepository commandRepository)
    {
        _commandRepository = commandRepository;
    }

    public Command CreateCommandRobot(
        PointDto startPoint,
        IReadOnlyCollection<DirectionStep> commands
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

        // Publish message.
        
        return command;
    }
}
