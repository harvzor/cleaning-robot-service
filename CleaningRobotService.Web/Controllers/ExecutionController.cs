using CleaningRobotService.BusinessLogic.Services;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.DataPersistence.Repositories;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Dtos.Output;
using CleaningRobotService.Web.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.Controllers;

public class ExecutionController : BaseController
{
    private readonly ICommandRobotService _commandRobotService;
    private readonly ICommandRobotRepository _commandRobotRepository;
    private readonly IExecutionRepository _executionRepository;
    
    public ExecutionController(
        ICommandRobotService commandRobotService,
        ICommandRobotRepository commandRobotRepository,
        IExecutionRepository executionRepository)
    {
        _commandRobotService = commandRobotService;
        _commandRobotRepository = commandRobotRepository;
        _executionRepository = executionRepository;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExecutionDto))]
    [ProducesDefaultResponseType]
    public ActionResult<CommandRobotDto> GetExecution([FromRoute] Guid id)
    {
        Execution? execution = _executionRepository.GetById(id: id);

        if (execution == null)
            return NotFound();
        
        return Ok(execution.ToDto());
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ExecutionDto>))]
    [ProducesDefaultResponseType]
    public ActionResult<IEnumerable<CommandRobotDto>> QueryExecutions([FromQuery] ExecutionGetDto dto)
    {
        // TODO: check if the dto.CommandRobotId even matches a record?
        IReadOnlyCollection<Execution> executions = _executionRepository.GetByCommandRobotId(dto.CommandRobotId);

        return Ok(executions.ToDtos());
    }
}
