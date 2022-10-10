using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Structs;

namespace CleaningRobotService.Web.Interfaces;

public interface IRobot
{
    Point StartPoint { get; set; }
    IEnumerable<Command> Commands { get; set; }
    IEnumerable<Point> CalculatePointsVisited();
}
