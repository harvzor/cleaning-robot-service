using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace CleaningRobotService.DataPersistence.Models;

public class Command : BaseModel
{
    /// <remarks>
    /// Mapped to columns as Postgres doesn't support complex objects.
    /// </remarks>
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
    
    public virtual List<DirectionStep> DirectionSteps { get; set; } = new();
}
