using CleaningRobotService.BusinessLogic.Services;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.DataPersistence.Repositories;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Dtos.Output;
using CleaningRobotService.Web.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.Controllers;

public class CommandsController : BaseController
{
    private readonly ICommandRobotService _commandRobotService;
    private readonly ICommandRobotRepository _commandRobotRepository;
    private readonly IExecutionRepository _executionRepository;
    
    public CommandsController(
        ICommandRobotService commandRobotService,
        ICommandRobotRepository commandRobotRepository,
        IExecutionRepository executionRepository
    )
    {
        _commandRobotService = commandRobotService;
        _commandRobotRepository = commandRobotRepository;
        _executionRepository = executionRepository;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandRobotDto))]
    [ProducesDefaultResponseType]
    public ActionResult<CommandRobotDto> CreateCommandRobot([FromBody] CommandRobotPostDto body)
    {
        CommandRobot commandRobot = _commandRobotService
            .CreateCommandRobot(
                startPoint: body.Start,
                commands: body.Commands
            );
        
        return Ok(commandRobot.ToDto());
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandRobotDto))]
    [ProducesDefaultResponseType]
    public ActionResult<CommandRobotDto> GetCommandRobots([FromRoute] Guid id)
    {
        CommandRobot? commandRobot = _commandRobotRepository.GetById(id: id);

        if (commandRobot == null)
            return NotFound();
        
        return Ok(commandRobot.ToDto());
    }
    
    
    
    [HttpGet("{id:guid}/executions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ExecutionDto>))]
    [ProducesDefaultResponseType]
    public ActionResult<IEnumerable<CommandRobotDto>> GetExecutions([FromRoute] Guid id)
    {
        // TODO: check if the dto.CommandRobotId even matches a record?
        IReadOnlyCollection<Execution> executions = _executionRepository.GetByCommandRobotId(id);

        return Ok(executions.ToDtos());
    }
}
