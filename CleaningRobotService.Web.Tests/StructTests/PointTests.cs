using CleaningRobotService.Web.Structs;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Web.Tests.StructTests;

public class PointTests
{
    [Fact]
    public void GetDistance()
    {
        // Arrange
        
        Point p1 = new()
        {
            X = 1,
            Y = 2,
        };
        
        Point p2 = new()
        {
            X = 3,
            Y = 2,
        };
        
        // Act

        int distance = Point.GetDistance(p1, p2);
        
        // Assert

        distance.ShouldBe(2);
    }

    [Theory]
    [InlineData(1, 3, 2, true)] // Point is in middle of line.
    [InlineData(2, 3, 1, false)] // Point is before line.
    public void PointOnLine(int lineP1X, int lineP2X, int p3X, bool shouldBeOnLine)
    {
        // Arrange
        
        Point lineP1 = new()
        {
            X = lineP1X,
            Y = 0,
        };
        
        Point lineP2 = new()
        {
            X = lineP2X,
            Y = 0,
        };
        
        Point p3 = new()
        {
            X = p3X,
            Y = 0,
        };
        
        // Act

        bool isOnLine = Point.PointOnLine(pt1: lineP1, pt2: lineP2, pt: p3);
        
        // Assert

        isOnLine.ShouldBe(shouldBeOnLine);
    }
}