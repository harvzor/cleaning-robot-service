using System.ComponentModel.DataAnnotations.Schema;

namespace CleaningRobotService.Web.Models;

/// <summary>
/// A single task sent to the cleaning robot.
/// </summary>
public class Execution
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    /// <summary>
    /// When this <see cref="Execution"/> started.
    /// </summary>
    public DateTimeOffset TimeStamp { get; set; }
    
    ///<summary>
    /// How many command elements were received.
    /// </summary>
    /// <remarks>Documentation tasks says to call this "commmands".</remarks>
    public int Commands { get; set; }
    
    /// <summary>
    /// Number of indices/points cleaned.
    /// </summary>
    public int Result { get; set; }
    
    /// <summary>
    /// How long it took to calculate the <see cref="Result"/>.
    /// </summary>
    /// <remarks>I would normally go with <see cref="TimeSpan"/> for time but the task says "in seconds".</remarks>
    public float Duration { get; set; }
}
