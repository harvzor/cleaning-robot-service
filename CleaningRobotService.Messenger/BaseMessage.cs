namespace CleaningRobotService.Messenger;

public abstract class BaseMessage : IMessage
{
    public Guid Id { get; set; }
    
    /// <inheritdoc cref="IMessage.CreatedAt"/>
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <inheritdoc cref="IMessage.ModifiedAt"/>
    public DateTimeOffset ModifiedAt { get; set; }
    
    /// <inheritdoc cref="IMessage.DeletedAt"/>
    public DateTimeOffset? DeletedAt { get; set; }
}