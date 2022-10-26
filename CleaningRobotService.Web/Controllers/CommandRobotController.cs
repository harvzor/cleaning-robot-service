using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.DataPersistence.Repositories;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Dtos.Output;
using CleaningRobotService.Web.Mappers;
using CleaningRobotService.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.Controllers;

public class CommandRobotController : BaseController
{
    private readonly CommandRobotService _commandRobotService;
    
    // TODO: change this so the service is injected instead.
    public CommandRobotController(IExecutionRepository executionRepository)
    {
        _commandRobotService = new CommandRobotService(repository: executionRepository);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandRobotDto))]
    [ProducesDefaultResponseType]
    public ActionResult<CommandRobotDto> CreateCommandRobot([FromBody] CommandRobotPostDto body)
    {
        Execution execution = _commandRobotService.CreateCommandRobot(body: body);
        
        return Ok(execution.ToDto());
    }
}
