using System.Net;
using CleaningRobotService.RobotCommander.ControllerGroups.Health.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleaningRobotService.RobotCommander.ControllerGroups.Health;

[Route("/health")]
public class HealthController : Controller
{
    private readonly HealthCheckService _healthCheckService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    
    public HealthController(HealthCheckService healthCheckService, IWebHostEnvironment hostingEnvironment)
    {
        _healthCheckService = healthCheckService;
        _hostingEnvironment = hostingEnvironment;
    }
     
    /// <summary>
    /// Get health.
    /// </summary>
    /// <remarks>Provides an indication about the health of the API</remarks>
    /// <response code="200">API is healthy</response>
    /// <response code="503">API is unhealthy or in degraded state</response>
    [HttpGet]
    [ProducesResponseType(typeof(HealthReportDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get()
    {
        var report = await _healthCheckService.CheckHealthAsync();

        if (_hostingEnvironment.IsDevelopment())
        {
            return report.Status == HealthStatus.Healthy
                ? Ok(report.ToDto())
                : StatusCode((int)HttpStatusCode.ServiceUnavailable, report.ToDto());
        }
        
        return report.Status == HealthStatus.Healthy ? Ok("Healthy") : StatusCode((int)HttpStatusCode.ServiceUnavailable, "Unhealthy");
    }
}
