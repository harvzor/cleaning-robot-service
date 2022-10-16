using System.Drawing;

namespace CleaningRobotService.Web.Objects;

public class Grid
{
    private readonly int _gridWidth;
    private readonly bool[,] _pointsVisited;
    public readonly Point GridOffset;

    public Grid(int gridWidth = 500, Point gridOffset = new())
    {
        _gridWidth = gridWidth;
        _pointsVisited = new bool[gridWidth, gridWidth];
        GridOffset = gridOffset;
    }

    public void AddPoint(Point point)
    {
        int offsetX = point.X - _gridWidth * GridOffset.X;
        int offsetY = point.Y - _gridWidth * GridOffset.Y;
        _pointsVisited[offsetX, offsetY] = true;
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
                        X = x + _gridWidth * GridOffset.X,
                        Y = y + _gridWidth * GridOffset.Y,
                    };
                }
            }
        }
    }
}
