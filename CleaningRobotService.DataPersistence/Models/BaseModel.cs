using System.ComponentModel.DataAnnotations;

namespace CleaningRobotService.DataPersistence.Models;

public class BaseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// When this this was saved.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Who created this item (user ID/Service name).
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string CreatedBy { get; set; } = "";
    
    /// <summary>
    /// When this this was saved.
    /// </summary>
    public DateTimeOffset ModifiedAt { get; set; }

    /// <summary>
    /// Who updated this item (user ID/Service name).
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string ModifiedBy { get; set; } = "";
    
    /// <summary>
    /// If null, this item has not been deleted. Otherwise, the date tells us when it was deleted.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Who deleted this item (user ID/Service name)  
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string? DeletedBy { get; set; }
}
