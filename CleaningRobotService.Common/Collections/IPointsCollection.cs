using System.Drawing;

namespace CleaningRobotService.Common.Collections;

/// <summary>
/// A 2D square grid of points which have been visited at least once.
/// </summary>
public interface IPointsCollections
{
    void AddPoint(Point point);
    IEnumerable<Point> GetPoints();
    int Count();
}
