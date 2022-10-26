using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;

namespace CleaningRobotService.DataPersistence.Models;

public class CommandRobot : BaseModel
{
    public Point StartPoint { get; set; }

    public List<CommandDto> Commands { get; set; } = new();
}
