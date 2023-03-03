using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.DataPersistence.Repositories;
using CleaningRobotService.Messenger;
using CleaningRobotService.RobotCommander.Messages;

namespace CleaningRobotService.Web.MessageHandlers;

public class CommandResultMessageHandler : IMessageHandler<CommandResultMessage>
{
    private readonly IExecutionRepository _executionRepository;

    public CommandResultMessageHandler(IExecutionRepository executionRepository)
    {
        _executionRepository = executionRepository;
    }

    public void Handle(CommandResultMessage commandResultMessage)
    {
        int? result = null;
        
        Execution? execution = _executionRepository
            .QueryObjectGraph(
                filter: x => x.Id == commandResultMessage.Id,
                includeChildren: x => x.Command
            )
            .FirstOrDefault();

        // If this message has been consumed before, ignore it.
        if (execution != null && execution.ModifiedAt >= commandResultMessage.ModifiedAt)
            return;

        if (execution == null)
        {
            execution = new Execution
            {
                Id = commandResultMessage.Id,
            };
            
            _executionRepository.Add(execution);
        }

        execution.CreatedAt = commandResultMessage.CreatedAt;
        execution.ModifiedAt = commandResultMessage.ModifiedAt;
        execution.Result = commandResultMessage.PointsVisited;
        execution.Duration = commandResultMessage.CalculationTime;
        execution.CommandId = commandResultMessage.CommandId;
        // execution.Commands =
        
        _executionRepository.Save();
    }
}