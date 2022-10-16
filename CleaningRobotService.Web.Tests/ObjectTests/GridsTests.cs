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
        
        //     0  1  2
        //  0 [ ][ ][ ]
        //  1 [ ][ ][ ]
        //  2 [ ][ ][ ]

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
    public void GridsTest_MultipleColumns()
    {
        // Arrange

        Grids grids = new Grids(gridWidth: 3);
        
        //     0  1  2
        //  0 [ ][ ][ ]
        //  1 [ ][ ][ ]
        //  2 [ ][ ][ ]

        Point[] pointsToAdd =
        {
            new Point(x: 0, y: 0),
            new Point(x: 3, y: 0), // Requires an extra grid to the right.
            // new Point(x: -1, y: 0), // Requires an extra grid to the left.
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
    public void GridsTest_MultipleRows()
    {
        // Arrange

        Grids grids = new Grids(gridWidth: 3);
        
        //     0  1  2
        //  0 [ ][ ][ ]
        //  1 [ ][ ][ ]
        //  2 [ ][ ][ ]

        Point[] pointsToAdd =
        {
            new Point(x: 0, y: 0),
            new Point(x: 0, y: 3), // Requires an extra grid added under.
            // new Point(x: 0, y: -1), // Requires an extra grid added above.
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
