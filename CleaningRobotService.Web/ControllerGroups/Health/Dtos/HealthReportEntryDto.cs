namespace CleaningRobotService.Web.ControllerGroups.Health.Dtos;

public struct HealthReportEntryDto
{
    /// <summary>
    /// Additional key-value pairs describing the health of the component.
    /// </summary>
    public IReadOnlyDictionary<string, object> Data { get; set; }

    /// <summary>
    /// Human-readable description of the status of the component that was checked.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Health check execution duration.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Health status of the component that was checked.
    /// </summary>
    public HealthStatusDto Status { get; set; }

    /// <summary>
    /// Tags associated with the health check.
    /// </summary>
    public IEnumerable<string> Tags { get; set; }
}
