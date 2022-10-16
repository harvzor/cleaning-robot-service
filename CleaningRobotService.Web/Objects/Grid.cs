using System.Drawing;

namespace CleaningRobotService.Web.Objects;

/// <summary>
/// A 2D square grid of points which have been visited at least once.
/// </summary>
// TODO: implement IEnumerable?
public class Grid
{
    private readonly int _gridWidth;
    private readonly bool[,] _pointsVisited;
    private readonly Point _gridOffset;
    private int _count = 0;
    
    /// <param name="gridWidth">
    /// The width and height of the grid.
    /// e.g. a width of 3 would create a 3x3 structure that supports Points [0,0] to [2,2].
    /// </param>
    /// <param name="gridOffset">
    /// Setting this allows you to create a grid which supports Points larger than the <param name="gridWidth"></param>.
    /// e.g. an offset of [-1,-1] and a <param name="gridWidth"></param> of 3 allows you to store from [-3,-3] to [-1,-1].
    /// </param>
    public Grid(int gridWidth = 500, Point gridOffset = new())
    {
        _gridWidth = gridWidth;
        _pointsVisited = new bool[gridWidth, gridWidth];
        _gridOffset = gridOffset;
    }

    public void AddPoint(Point point)
    {
        int offsetX = point.X - _gridWidth * _gridOffset.X;
        int offsetY = point.Y - _gridWidth * _gridOffset.Y;

        if (!_pointsVisited[offsetX, offsetY])
        {
            _pointsVisited[offsetX, offsetY] = true;
            _count++;
        }
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
                        X = x + _gridWidth * _gridOffset.X,
                        Y = y + _gridWidth * _gridOffset.Y,
                    };
                }
            }
        }
    }

    public int Count() => _count;
}
