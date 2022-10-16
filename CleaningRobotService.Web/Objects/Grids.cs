using System.Drawing;

namespace CleaningRobotService.Web.Objects;

public class Grids
{
    private readonly int _gridWidth;
    private readonly List<Grid> _grids = new();
    
    public Grids(int gridWidth = 500)
    {
        _gridWidth = gridWidth;
    }

    public static int CalculateGridNumber(int xOrY, int gridWidth)
    {
        // I'm sure there's a proper formula for this using modulo?
        int divide = xOrY / gridWidth;

        if (xOrY < 0 && gridWidth > 1)
            return divide - 1;

        return divide;
    }
    
    public void AddPoint(Point point)
    {
        int gridColumnIndex = CalculateGridNumber(xOrY: point.X, gridWidth: _gridWidth);
        int gridRowIndex = CalculateGridNumber(xOrY: point.Y, gridWidth: _gridWidth);

        Grid? matchingGrid = _grids
            .Where(x => x.GridOffset.X == gridColumnIndex && x.GridOffset.Y == gridRowIndex)
            .FirstOrDefault();

        if (matchingGrid == null)
        {
            matchingGrid = new Grid(gridWidth: _gridWidth, gridOffset: new Point(x: gridColumnIndex, y: gridRowIndex));

            _grids.Add(matchingGrid);
        }

        matchingGrid.AddPoint(point);
    }
    
    public IEnumerable<Point> GetPoints()
    {
        foreach (Grid grid in _grids)
        {
            foreach (Point point in grid.GetPoints())
            {
                yield return point;
            }
        }
    }
}
