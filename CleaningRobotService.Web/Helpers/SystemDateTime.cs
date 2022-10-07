namespace CleaningRobotService.Web.Helpers;

/// <summary>
/// Reference this to get the current time so time can be frozen in testing.
/// </summary>
public static class SystemDateTime
{
    [ThreadStatic]
    private static DateTime? FixedDateTime;

    public static DateTime UtcNow
    {
        get => FixedDateTime ?? DateTime.UtcNow;
        set => FixedDateTime = value;
    }

    /// <summary>
    /// Should only be used for testing.
    /// </summary>
    public static void SetConstant()
    {
        UtcNow = UtcNow;
    }
}
