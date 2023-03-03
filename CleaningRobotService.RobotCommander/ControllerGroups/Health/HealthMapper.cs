using CleaningRobotService.RobotCommander.ControllerGroups.Health.Dtos;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleaningRobotService.RobotCommander.ControllerGroups.Health;

public static class HealthMapper
{
    public static HealthReportDto ToDto(this HealthReport healthReport) => new()
    {
        Entries = healthReport.Entries.ToDtos(),
        Status = healthReport.Status.ToDto(),
        TotalDuration = healthReport.TotalDuration,
    };

    private static HealthReportEntryDto ToDto(this HealthReportEntry healthReportEntry) => new HealthReportEntryDto
    {
        Data = healthReportEntry.Data,
        Status = healthReportEntry.Status.ToDto(),
        Description = healthReportEntry.Description,
        Duration = healthReportEntry.Duration,
        Tags = healthReportEntry.Tags,
    };
    
    private static IReadOnlyDictionary<string, HealthReportEntryDto> ToDtos(this IReadOnlyDictionary<string, HealthReportEntry> healthReportEntries) =>
        healthReportEntries.ToDictionary(x => x.Key, x => x.Value.ToDto());
    
    private static HealthStatusDto ToDto(this HealthStatus healthStatus)
        => (HealthStatusDto)Enum.Parse(typeof(HealthStatusDto), healthStatus.ToString());
}