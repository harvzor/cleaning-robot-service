using System.Drawing;

namespace CleaningRobotService.Web.Objects;

public class Grid
{
    private readonly int _gridWidth;
    private readonly bool[,] _pointsVisited;

    public Grid(int gridWidth = 500)
    {
        _gridWidth = gridWidth;
        _pointsVisited = new bool[gridWidth, gridWidth];
    }

    public void AddPoint(Point point)
    {
        _pointsVisited[_gridWidth / 2 + point.X, _gridWidth / 2 + point.Y] = true;
    }

    public IEnumerable<Point> GetPoints()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridWidth; y++)
            {
                if (_pointsVisited[x, y])
                {
                    yield return new Point
                    {
                        X = x - _gridWidth / 2,
                        Y = y - _gridWidth / 2,
                    };
                }
            }
        }
    }
}
