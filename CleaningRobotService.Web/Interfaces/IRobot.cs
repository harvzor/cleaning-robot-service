using System.Drawing;
using CleaningRobotService.Web.Dtos.Input;

namespace CleaningRobotService.Web.Interfaces;

public interface IRobot
{
    Point StartPoint { get; set; }
    IEnumerable<Command> Commands { get; set; }
    void CalculatePointsVisited();
    IEnumerable<Point> GetPointsVisited();
    int CountPointsVisited();
}
