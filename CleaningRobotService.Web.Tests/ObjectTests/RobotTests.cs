using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Objects;
using CleaningRobotService.Web.Structs;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Web.Tests.ObjectTests;

public class RobotTests
{
    [Fact]
    public void CalculateIndicesCleanedTest()
    {
        // Arrange

        Robot robot = new(startPoint: new Point
        {
            X = 10,
            Y = 22,
        });
        
        // Act
        
        int result = robot.CalculateIndicesVisited(commands: new List<Command>()
        {
            new Command
            {
                Direction = DirectionEnum.east,
                Steps = 2,
            },
            new Command
            {
                Direction = DirectionEnum.north,
                Steps = 1,
            },
        });
        
        // Assert

        result.ShouldBe(4);
    }
    
    [Fact]
    public void CalculateIndicesCleaned_EnsureSameStepCountedOnce()
    {
        // Arrange

        Robot robot = new(startPoint: new Point
        {
            X = 0,
            Y = 0,
        });
        
        // Act
        
        int result = robot.CalculateIndicesVisited(commands: new List<Command>()
        {
            new Command
            {
                Direction = DirectionEnum.east,
                Steps = 1,
            },
            // Go back on itself.
            new Command
            {
                Direction = DirectionEnum.west,
                Steps = 1,
            },
        });

        // Assert

        result.ShouldBe(2);
    }
}