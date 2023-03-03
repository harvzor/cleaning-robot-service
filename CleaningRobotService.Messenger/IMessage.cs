namespace CleaningRobotService.Messenger;

public interface IMessage
{
    Guid Id { get; set; }

    /// <summary>
    /// When this this was saved.
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// When this this was saved.
    /// </summary>
    DateTimeOffset ModifiedAt { get; set; }

    /// <summary>
    /// If null, this item has not been deleted. Otherwise, the date tells us when it was deleted.
    /// </summary>
    DateTimeOffset? DeletedAt { get; set; }
}
