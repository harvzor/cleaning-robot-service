using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Robots;

namespace CleaningRobotService.Common.Factories;

/// <remarks>
/// Since there are different implementations of IRobot, this factory selects the best one.
/// May seem a little bit unnecessary since the same implementation is always used, but this allows for easy switching.
/// </remarks>
public class RobotFactory
{
    public IRobot GetRobot(Point startPoint, IEnumerable<DirectionStep> commands)
    {
        // IRobot robot = new RobotPoints
        // IRobot robot = new RobotLines
        IRobot robot = new RobotDictionaryLines
        // IRobot robot = new RobotGrid
        // IRobot robot = new RobotSwarm
        (
            startPoint: startPoint,
            commands: commands
        );
        
        return robot;
    }
}
