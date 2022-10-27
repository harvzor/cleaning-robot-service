using CleaningRobotService.BusinessLogic.Services;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.DataPersistence.Repositories;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Dtos.Output;
using CleaningRobotService.Web.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.Controllers;

public class CommandRobotController : BaseController
{
    private readonly ICommandRobotService _commandRobotService;
    private readonly ICommandRobotRepository _commandRobotRepository;
    
    public CommandRobotController(ICommandRobotService commandRobotService, ICommandRobotRepository commandRobotRepository)
    {
        _commandRobotService = commandRobotService;
        _commandRobotRepository = commandRobotRepository;
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
    public ActionResult<CommandRobotDto> GetCommandRobot([FromRoute] Guid id)
    {
        CommandRobot? commandRobot = _commandRobotRepository.GetById(id: id);

        if (commandRobot == null)
            return NotFound();
        
        return Ok(commandRobot.ToDto());
    }
}
