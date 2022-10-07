using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Dtos.Output;
using CleaningRobotService.Web.Mappers;
using CleaningRobotService.Web.Models;
using CleaningRobotService.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.Controllers;

public class CommandRobotController : BaseController
{
    private readonly CommandRobotService CommandRobotService;
    
    protected CommandRobotController(ServiceDbContext context) : base(context)
    {
        this.CommandRobotService = new CommandRobotService(context: context);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandRobotDto))]
    [ProducesDefaultResponseType]
    public ActionResult<CommandRobotDto> CreateCommandRobot([FromBody] CommandRobotPostDto body)
    {
        Execution execution = this.CommandRobotService.CreateCommandRobot(body: body);
        
        return Ok(execution.ToDto());
    }
}