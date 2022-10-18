using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;

namespace CleaningRobotService.Common.Interfaces;

public interface IRobot
{
    Point StartPoint { get; set; }
    IEnumerable<CommandDto> Commands { get; set; }
    void CalculatePointsVisited();
    IEnumerable<Point> GetPointsVisited();
    int CountPointsVisited();
}
