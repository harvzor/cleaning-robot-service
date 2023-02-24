using CleaningRobotService.BusinessLogic.Services;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.DataPersistence.Repositories;
using CleaningRobotService.Web.ControllerGroups.Base;
using CleaningRobotService.Web.ControllerGroups.Executions;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Dtos.Output;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.ControllerGroups.Commands;

public class CommandsController : BaseController
{
    private readonly ICommandRobotService _commandRobotService;
    private readonly ICommandRepository _commandRepository;
    private readonly IExecutionRepository _executionRepository;
    
    public CommandsController(
        ICommandRobotService commandRobotService,
        ICommandRepository commandRepository,
        IExecutionRepository executionRepository
    )
    {
        _commandRobotService = commandRobotService;
        _commandRepository = commandRepository;
        _executionRepository = executionRepository;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandDto))]
    [ProducesDefaultResponseType]
    public ActionResult<CommandDto> CreateCommand([FromBody] CommandsPostDto body)
    {
        Command command = _commandRobotService
            .CreateCommandRobot(
                startPoint: body.Start,
                commands: body.Commands
            );
        
        return Ok(command.ToDto());
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandDto))]
    [ProducesDefaultResponseType]
    public ActionResult<CommandDto> GetCommands([FromRoute] Guid id)
    {
        Command? commandRobot = _commandRepository.GetById(id: id);

        if (commandRobot == null)
            return NotFound();
        
        return Ok(commandRobot.ToDto());
    }
    
    
    
    [HttpGet("{id:guid}/executions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ExecutionDto>))]
    [ProducesDefaultResponseType]
    public ActionResult<IEnumerable<ExecutionDto>> GetExecutions([FromRoute] Guid id)
    {
        // TODO: check if the dto.CommandId even matches a record?
        IReadOnlyCollection<Execution> executions = _executionRepository.GetByCommandRobotId(id);

        return Ok(executions.ToDtos());
    }
}
