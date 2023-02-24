namespace CleaningRobotService.Web.ControllerGroups.Health.Dtos;

/// <summary>
/// Represents the reported status of a health check result.
/// </summary>
/// <remarks>
/// <para>
/// A status of <see cref="Unhealthy"/> should be considered the default value for a failing health check. Application
/// developers may configure a health check to report a different status as desired.
/// </para>
/// <para>
/// The values of this enum or ordered from least healthy to most healthy. So <see cref="HealthStatusDto.Degraded"/> is
/// greater than <see cref="HealthStatusDto.Unhealthy"/> but less than <see cref="HealthStatusDto.Healthy"/>.
/// </para>
/// </remarks>
public enum HealthStatusDto
{
    /// <summary>
    /// Indicates that the health check determined that the component was unhealthy, or an unhandled
    /// exception was thrown while executing the health check.
    /// </summary>
    Unhealthy = 0,

    /// <summary>
    /// Indicates that the health check determined that the component was in a degraded state.
    /// </summary>
    Degraded = 1,

    /// <summary>
    /// Indicates that the health check determined that the component was healthy.
    /// </summary>
    Healthy = 2,
}
