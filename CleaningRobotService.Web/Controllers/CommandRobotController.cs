using CleaningRobotService.Common.Services;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Dtos.Output;
using CleaningRobotService.Web.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.Controllers;

public class CommandRobotController : BaseController
{
    private readonly ICommandRobotService _commandRobotService;
    
    public CommandRobotController(ICommandRobotService commandRobotService)
    {
        _commandRobotService = commandRobotService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandRobotDto))]
    [ProducesDefaultResponseType]
    public ActionResult<CommandRobotDto> CreateCommandRobot([FromBody] CommandRobotPostDto body)
    {
        Execution execution = _commandRobotService
            .CreateCommandRobot(
                startPoint: body.Start,
                commands: body.Commands
            );
        
        return Ok(execution.ToDto());
    }
}
