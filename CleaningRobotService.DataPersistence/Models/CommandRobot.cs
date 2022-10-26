using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace CleaningRobotService.DataPersistence.Models;

public class CommandRobot : BaseModel
{
    [NotMapped]
    public Point StartPoint
    {
        get => new(x: StartPointX, y: StartPointY);
        init
        {
            StartPointX = value.X;
            StartPointY = value.Y;
        }
    }

    public int StartPointX { get; set; }
    
    public int StartPointY { get; set; }
    
    public virtual List<CommandRobotCommand> Commands { get; set; } = new();
}
