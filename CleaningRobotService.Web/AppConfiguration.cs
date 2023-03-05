using System.ComponentModel.DataAnnotations;
using CleaningRobotService.Messenger;

namespace CleaningRobotService.Web;

public class AppConfiguration
{
    [Required] public string DatabaseConnectionString { get; set; } = default!;
    
    [Required]
    public KafkaConfiguration Kafka { get; set; }
}