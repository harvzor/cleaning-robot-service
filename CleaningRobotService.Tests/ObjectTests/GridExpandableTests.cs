using System.Drawing;
using CleaningRobotService.Common.Objects;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Tests.ObjectTests;

public class GridExpandableTests
{
    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(1, 1, 1)]
    [InlineData(-1, 1, -1)]
    [InlineData(0, 3, 0)]
    [InlineData(3, 3, 1)]
    [InlineData(-1, 3, -1)]
    public void Grids_CalculateGridNumber(int xOrY, int gridWidth, int expectedGridNumber)
    {
        GridExpandable.CalculateGridNumber(xOrY: xOrY, gridWidth: gridWidth).ShouldBe(expectedGridNumber);
    }
    
    [Fact]
    public void GridsTest_SingleGrid()
    {
        // Arrange

        GridExpandable gridExpandable = new GridExpandable(gridWidth: 3);
        
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
            gridExpandable.AddPoint(pointToAdd);
        
        Point[] points = gridExpandable.GetPoints().ToArray();
        
        // Assert

        points.Length.ShouldBe(pointsToAdd.Length);
        
        foreach (Point pointToAdd in pointsToAdd)
            points.ShouldContain(pointToAdd);
    }
    
    [Fact]
    public void GridsTest_MultipleColumns()
    {
        // Arrange

        GridExpandable gridExpandable = new GridExpandable(gridWidth: 3);
        
        //     0  1  2
        //  0 [ ][ ][ ]
        //  1 [ ][ ][ ]
        //  2 [ ][ ][ ]

        Point[] pointsToAdd =
        {
            new Point(x: 0, y: 0),
            new Point(x: 3, y: 0), // Requires an extra grid to the right.
            new Point(x: -1, y: 0), // Requires an extra grid to the left.
        };

        // Act
        
        foreach (Point pointToAdd in pointsToAdd)
            gridExpandable.AddPoint(pointToAdd);
        
        Point[] points = gridExpandable.GetPoints().ToArray();
        
        // Assert

        points.Length.ShouldBe(pointsToAdd.Length);
        
        foreach (Point pointToAdd in pointsToAdd)
            points.ShouldContain(pointToAdd);
    }
    
    [Fact]
    public void GridsTest_MultipleRows()
    {
        // Arrange

        GridExpandable gridExpandable = new GridExpandable(gridWidth: 3);
        
        //     0  1  2
        //  0 [ ][ ][ ]
        //  1 [ ][ ][ ]
        //  2 [ ][ ][ ]

        Point[] pointsToAdd =
        {
            new Point(x: 0, y: 0),
            new Point(x: 0, y: 3), // Requires an extra grid added under.
            new Point(x: 0, y: -1), // Requires an extra grid added above.
        };

        // Act
        
        foreach (Point pointToAdd in pointsToAdd)
            gridExpandable.AddPoint(pointToAdd);
        
        Point[] points = gridExpandable.GetPoints().ToArray();
        
        // Assert

        points.Length.ShouldBe(pointsToAdd.Length);
        
        foreach (Point pointToAdd in pointsToAdd)
            points.ShouldContain(pointToAdd);
    }
}
