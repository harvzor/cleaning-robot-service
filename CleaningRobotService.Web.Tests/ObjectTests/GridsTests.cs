using System.Drawing;
using CleaningRobotService.Web.Objects;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Web.Tests.ObjectTests;

public class GridsTests
{
    [Fact]
    public void GridsTest_SingleGrid()
    {
        // Arrange

        Grids grids = new Grids(gridWidth: 3);
        
        /*
             -1  0  1
           1 [ ][ ][ ]
           0 [ ][ ][ ]
          -1 [ ][ ][ ]
        */

        Point[] pointsToAdd =
        {
            new Point(x: 0, y: 0),
        };

        // Act
        
        foreach (Point pointToAdd in pointsToAdd)
            grids.AddPoint(pointToAdd);
        
        Point[] points = grids.GetPoints().ToArray();
        
        // Assert

        points.Length.ShouldBe(pointsToAdd.Length);
        
        foreach (Point pointToAdd in pointsToAdd)
            points.ShouldContain(pointToAdd);
    }
    
    [Fact]
    public void GridsTest_MultipleGrids()
    {
        // Arrange

        Grids grids = new Grids(gridWidth: 3);
        
        /*
             -1  0  1
           1 [ ][ ][ ]
           0 [ ][ ][ ]
          -1 [ ][ ][ ]
        */

        Point[] pointsToAdd =
        {
            new Point(x: 0, y: 0),
            new Point(x: 2, y: 0), // Outside the range of a single grid.
        };

        // Act
        
        foreach (Point pointToAdd in pointsToAdd)
            grids.AddPoint(pointToAdd);
        
        Point[] points = grids.GetPoints().ToArray();
        
        // Assert

        points.Length.ShouldBe(pointsToAdd.Length);
        
        foreach (Point pointToAdd in pointsToAdd)
            points.ShouldContain(pointToAdd);
    }
}
