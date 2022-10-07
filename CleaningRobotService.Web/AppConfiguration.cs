using System.ComponentModel.DataAnnotations;

namespace CleaningRobotService.Web;

public class AppConfiguration
{
    [Required] public string DatabaseConnectionString { get; set; } = default!;
}