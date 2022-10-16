using System.Drawing;

namespace CleaningRobotService.Web.Objects;

public class Grids
{
    private readonly int _gridWidth;
    private readonly List<List<Grid>> _grids = new();
    
    public Grids(int gridWidth = 500)
    {
        _gridWidth = gridWidth;
    }
    
    private float GridWidthHalf => (float)_gridWidth / 2;

    private int CalculateGridNumber(int xOrY)
    {
        // Example: xOrY = 1, _gridWidth = 500
        // (1 - (1 % _gridWidth)) / _gridWidth = 0
        // Example: xOrY = 501, _gridWidth = 500
        // (501 - (501 % _gridWidth)) / _gridWidth = 1
        // return (xOrY - (xOrY % _gridWidth)) / _gridWidth;
        
        // Example: xOrY = 1, _gridWidth = 3
        // (1 - (1 % _gridWidthHalf)) / _gridWidthHalf = 0
        // Example: xOrY = 2, _gridWidth = 3
        // (2 - (2 % _gridWidthHalf)) / _gridWidthHalf = 1
        return (int)Math.Round((xOrY - (xOrY % GridWidthHalf)) / GridWidthHalf);
    }
    
    public void AddPoint(Point point)
    {
        int gridColumnIndex = CalculateGridNumber(point.X);
        int gridRowIndex = CalculateGridNumber(point.Y);
        
        while (_grids.Count <= gridRowIndex)
            _grids.Add(new List<Grid>());

        while (_grids[gridRowIndex].Count <= gridColumnIndex)
            _grids[gridRowIndex].Add(new Grid(gridWidth: _gridWidth));
        
        Point offsetPoint = new Point
        {
            X = (int)Math.Round(point.X % GridWidthHalf),
            Y = (int)Math.Round(point.Y % GridWidthHalf),
        };

        _grids[gridRowIndex][gridColumnIndex].AddPoint(offsetPoint);
    }
    
    public IEnumerable<Point> GetPoints()
    {
        int rowIndex = 0;
        foreach (List<Grid> row in _grids)
        {
            int columnIndex = 0;
            foreach (Grid grid in row)
            {
                foreach (Point point in grid.GetPoints())
                {
                    Point offsetPoint = new Point
                    {
                        X = point.X + (int)Math.Round(columnIndex * GridWidthHalf),
                        Y = point.Y + (int)Math.Round(rowIndex * GridWidthHalf),
                    };

                    yield return offsetPoint;
                }

                columnIndex++;
            }

            rowIndex++;
        }
    }
}
