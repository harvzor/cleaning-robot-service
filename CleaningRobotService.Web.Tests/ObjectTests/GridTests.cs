using System.Drawing;
using CleaningRobotService.Web.Objects;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Web.Tests.ObjectTests;

public class GridTests
{
    [Fact]
    public void GridTest()
    {
        // Arrange

        Grid grid = new Grid(gridWidth: 5);
        
        //     0  1  2  3  4
        //  0 [ ][ ][ ][ ][ ]
        //  1 [ ][ ][ ][ ][ ]
        //  2 [ ][ ][ ][ ][ ]
        //  3 [ ][ ][ ][ ][ ]
        //  4 [ ][ ][ ][ ][ ]

        Point[] pointsToAdd =
        {
            new Point(x: 0, y: 0),
            new Point(x: 2, y: 2),
            new Point(x: 4, y: 4),
        };

        // Act
        
        foreach (Point pointToAdd in pointsToAdd)
            grid.AddPoint(pointToAdd);
        
        Point[] points = grid.GetPoints().ToArray();
        
        // Assert

        points.Length.ShouldBe(pointsToAdd.Length);
        
        foreach (Point pointToAdd in pointsToAdd)
            points.ShouldContain(pointToAdd);
    }
}
