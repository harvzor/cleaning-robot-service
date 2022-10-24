using System.Drawing;
using CleaningRobotService.Common.Structures;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Tests.StructureTests;

public class LineTests
{
    [Fact]
    public void Line_CalculatePoints_JustAPoint()
    {
        // Arrange

        Line line = new
        (
            start: new Point(1, 1),
            end: new Point(1, 1)
        );

        // Act

        List<Point> points = line.CalculatePoints();
        
        // Assert

        points.Count.ShouldBe(1);
        points.ShouldContain(new Point(1, 1));
    }
    
    [Fact]
    public void Line_CalculatePoints_VerticalTest()
    {
        // Arrange
        
        Line line = new
        (
            start: new Point(1, -1),
            end: new Point(1, 1)
        );
        
        // Act

        List<Point> points = line.CalculatePoints();
        
        // Assert

        points.Count.ShouldBe(3);
        points.ShouldContain(new Point(1, -1));
        points.ShouldContain(new Point(1, 0));
        points.ShouldContain(new Point(1, 1));
    }
    
    [Fact]
    public void Line_CalculatePoints_HorizontalTest()
    {
        // Arrange
        
        Line line = new
        (
            start: new Point(-1, 1),
            end: new Point(1, 1)
        );
        
        // Act

        List<Point> points = line.CalculatePoints();
        
        // Assert

        points.Count.ShouldBe(3);
        points.ShouldContain(new Point(-1, 1));
        points.ShouldContain(new Point(0, 1));
        points.ShouldContain(new Point(1, 1));
    }
}