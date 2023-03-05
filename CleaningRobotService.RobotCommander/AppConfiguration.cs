using System.ComponentModel.DataAnnotations;
using CleaningRobotService.Messenger;

namespace CleaningRobotService.RobotCommander;

public class AppConfiguration
{
    [Required]
    public KafkaConfiguration Kafka { get; set; }
}