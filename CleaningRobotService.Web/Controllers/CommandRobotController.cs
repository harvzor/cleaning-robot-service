using CleaningRobotService.DataPersistence;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Dtos.Output;
using CleaningRobotService.Web.Mappers;
using CleaningRobotService.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.Controllers;

public class CommandRobotController : BaseController
{
    private readonly CommandRobotService _commandRobotService;
    
    public CommandRobotController(ServiceDbContext context) : base(context)
    {
        _commandRobotService = new CommandRobotService(context: context);
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
