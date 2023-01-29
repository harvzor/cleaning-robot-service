using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.DataPersistence.Repositories;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Dtos.Output;
using CleaningRobotService.Web.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.Controllers;

public class ExecutionsController : BaseController
{
    private readonly IExecutionRepository _executionRepository;
    
    public ExecutionsController(IExecutionRepository executionRepository)
    {
        _executionRepository = executionRepository;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExecutionDto))]
    [ProducesDefaultResponseType]
    public ActionResult<ExecutionDto> GetExecution([FromRoute] Guid id)
    {
        Execution? execution = _executionRepository.GetById(id: id);

        if (execution == null)
            return NotFound();
        
        return Ok(execution.ToDto());
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ExecutionDto>))]
    [ProducesDefaultResponseType]
    public ActionResult<IEnumerable<ExecutionDto>> QueryExecutions([FromQuery] ExecutionGetDto dto)
    {
        // TODO: check if the dto.CommandId even matches a record?
        IReadOnlyCollection<Execution> executions = _executionRepository.GetByCommandRobotId(dto.CommandRobotId);

        return Ok(executions.ToDtos());
    }
}
