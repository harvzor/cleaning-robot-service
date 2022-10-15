using System.Drawing;
using CleaningRobotService.Web.Dtos.Input;

namespace CleaningRobotService.Web.Interfaces;

public interface IRobot
{
    Point StartPoint { get; set; }
    IEnumerable<Command> Commands { get; set; }
    IEnumerable<Point> CalculatePointsVisited();
}
