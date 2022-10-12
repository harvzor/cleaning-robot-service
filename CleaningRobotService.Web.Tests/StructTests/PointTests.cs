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
}