using System.ComponentModel.DataAnnotations.Schema;
using CleaningRobotService.Common.Enums;

namespace CleaningRobotService.DataPersistence.Models;

public class DirectionStep : BaseModel
{
    /// <summary>
    /// Which direction the robot should move.
    /// </summary>
    [Column(TypeName = "text")]
    public DirectionEnum Direction { get; set; }
    
    /// <summary>
    /// How many steps to take.
    /// </summary>
    public uint Steps { get; set; }
}
