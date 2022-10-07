using System.Diagnostics;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Dtos.Output;
using CleaningRobotService.Web.Helpers;
using CleaningRobotService.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.Controllers;

public class CommandRobotController : BaseController
{
    protected CommandRobotController(DbContext context) : base(context)
    {
    }

    private int CalculateIndicesCleaned(Point startPoint, List<Command> commands, out float calculationTime)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();
        
        Point currentPoint = startPoint;

        HashSet<Point> pointsVisited = new() { currentPoint };

        foreach (Command command in commands)
        {
            switch (command.Direction)
            {
                case DirectionEnum.North:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        currentPoint.Y++;
                        
                        if (!pointsVisited.Contains(currentPoint))
                            pointsVisited.Add(currentPoint);
                    }
                    
                    break;
                case DirectionEnum.East:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        currentPoint.X++;
                        
                        if (!pointsVisited.Contains(currentPoint))
                            pointsVisited.Add(currentPoint);
                    }
                    
                    break;
                case DirectionEnum.South:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        currentPoint.Y--;
                        
                        if (!pointsVisited.Contains(currentPoint))
                            pointsVisited.Add(currentPoint);
                    }
                    
                    break;
                case DirectionEnum.West:
                    for (int i = 0; i < command.Steps; i++)
                    {
                        currentPoint.X--;
                        
                        if (!pointsVisited.Contains(currentPoint))
                            pointsVisited.Add(currentPoint);
                    }
                    
                    break;
                default:
                    // TODO: replace with a proper exception (don't use Exception!)
                    throw new Exception("Command direction case not covered.");
            }
        }
        
        stopWatch.Stop();
        calculationTime = stopWatch.ElapsedMilliseconds * 1000;

        return pointsVisited.Count;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandRobotDto))]
    [ProducesDefaultResponseType]
    public ActionResult<CommandRobotDto> CreateCommandRobot([FromBody] CommandRobotPostDto body)
    {
        DateTimeOffset now = SystemDateTime.UtcNow;
        // TODO: Move to service.

        int result = CalculateIndicesCleaned(startPoint: body.Start, commands: body.Commands, out float calculationTime);
        
        Execution execution = new Execution
        {
            TimeStamp = now,
            Commands = body.Commands.Count,
            Result = result,
            Duration = calculationTime,
        };
        
        base.Context.Executions.Add(execution);
    }
}