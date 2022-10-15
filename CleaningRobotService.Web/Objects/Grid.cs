using System.Drawing;

namespace CleaningRobotService.Web.Objects;

public class Grid
{
    // Will allocate ~5GB of data?
    // const int squareWidth = 200000; // Array dimensions exceeded supported range
    private const int SquareWidth = 500;
    private readonly bool[,] _pointsVisited = new bool[SquareWidth, SquareWidth];

    public void AddPoint(Point point)
    {
        _pointsVisited[SquareWidth / 2 + point.X, SquareWidth / 2 + point.Y] = true;
    }

    public IEnumerable<Point> GetPoints()
    {
        for (int x = 0; x < SquareWidth; x++)
        {
            for (int y = 0; y < SquareWidth; y++)
            {
                if (_pointsVisited[x, y])
                {
                    yield return new Point
                    {
                        X = x - SquareWidth / 2,
                        Y = y - SquareWidth / 2,
                    };
                }
            }
        }
    }
}
