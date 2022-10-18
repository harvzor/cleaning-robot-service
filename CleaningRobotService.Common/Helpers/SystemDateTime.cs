namespace CleaningRobotService.Common.Helpers;

/// <summary>
/// Reference this to get the current time so time can be frozen in testing.
/// </summary>
public static class SystemDateTime
{
    [ThreadStatic]
    private static DateTime? _fixedDateTime;

    public static DateTime UtcNow
    {
        get => _fixedDateTime ?? DateTime.UtcNow;
        set => _fixedDateTime = value;
    }

    /// <summary>
    /// Should only be used for testing.
    /// </summary>
    public static void SetConstant()
    {
        UtcNow = UtcNow;
    }
}
