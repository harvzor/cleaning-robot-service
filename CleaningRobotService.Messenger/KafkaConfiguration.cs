using System.ComponentModel.DataAnnotations;

namespace CleaningRobotService.Messenger;

public class KafkaConfiguration
{
    [Required]
    public string BootstrapServers { get; set; } = default!;
}
