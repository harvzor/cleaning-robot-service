namespace CleaningRobotService.Web.Dtos.Output;

// TODO: super similar to BaseModel, maybe refactor to DRY this up?
public class BaseDto
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// When this this was saved.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    // /// <summary>
    // /// Who created this item (user ID/Service name).
    // /// </summary>
    // public string CreatedBy { get; set; } = "";
    
    /// <summary>
    /// When this this was saved.
    /// </summary>
    public DateTimeOffset ModifiedAt { get; set; }

    // /// <summary>
    // /// Who updated this item (user ID/Service name).
    // /// </summary>
    // public string ModifiedBy { get; set; } = "";
    
    /// <summary>
    /// If null, this item has not been deleted. Otherwise, the date tells us when it was deleted.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // /// <summary>
    // /// Who deleted this item (user ID/Service name)  
    // /// </summary>
    // public string? DeletedBy { get; set; }
}