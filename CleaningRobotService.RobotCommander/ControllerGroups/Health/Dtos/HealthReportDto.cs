namespace CleaningRobotService.RobotCommander.ControllerGroups.Health.Dtos;

/// <remarks>
/// Have to map <see cref="Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport"/> because it eventually contains an Exception which cannot be serialized.
/// </remarks>
public class HealthReportDto
{
    /// <summary>
    /// A <see cref="IReadOnlyDictionary{TKey, T}"/> containing the results from each health check.
    /// </summary>
    /// <remarks>
    /// The keys in this dictionary map the name of each executed health check to a <see cref="HealthReportEntryDto"/> for the
    /// result data returned from the corresponding health check.
    /// </remarks>
    public IReadOnlyDictionary<string, HealthReportEntryDto>? Entries { get; set; }

    /// <summary>
    /// A <see cref="HealthStatusDto"/> representing the aggregate status of all the health checks. The value of <see cref="Status"/>
    /// will be the most severe status reported by a health check. If no checks were executed, the value is always <see cref="HealthStatusDto.Healthy"/>.
    /// </summary>
    public HealthStatusDto Status { get; set; }

    /// <summary>
    /// Time the health check service took to execute.
    /// </summary>
    public TimeSpan TotalDuration { get; set; }
}