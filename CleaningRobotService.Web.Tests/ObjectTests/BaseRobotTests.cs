using System.Drawing;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Interfaces;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Web.Tests.ObjectTests;

public abstract class BaseRobotTests<TRobot> where TRobot : IRobot, new()
{
    [Fact]
    protected void CalculatePointsVisitedTest()
    {
        // Arrange

        IRobot robot = new TRobot();

        robot.StartPoint = new Point
        {
            X = 10,
            Y = 22,
        };
        robot.Commands = new List<Command>()
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
        };
        
        // Act

        Point[] pointsVisited = robot
            .CalculatePointsVisited()
            .ToArray();
        
        // Assert

        pointsVisited.Count().ShouldBe(4);
        pointsVisited.ShouldContain(new Point(x: 10, y: 22));
        pointsVisited.ShouldContain(new Point(x: 11, y: 22));
        pointsVisited.ShouldContain(new Point(x: 12, y: 22));
        pointsVisited.ShouldContain(new Point(x: 12, y: 23));
    }
    
    [Fact]
    protected void CalculatePointsVisited_SupportsNegativePoints()
    {
        // Arrange
        
        IRobot robot = new TRobot();

        robot.StartPoint = new Point
        {
            X = 0,
            Y = 0,
        };
        robot.Commands = new List<Command>()
        {
            new Command
            {
                Direction = DirectionEnum.west,
                Steps = 1,
            },
            // Go back on itself.
            new Command
            {
                Direction = DirectionEnum.south,
                Steps = 1,
            },
        };
        
        // Act

        Point[] pointsVisited = robot
            .CalculatePointsVisited()
            .ToArray();

        // Assert

        pointsVisited.Count().ShouldBe(3);
        pointsVisited.ShouldContain(new Point(x: 0, y: 0));
        pointsVisited.ShouldContain(new Point(x: -1, y: 0));
        pointsVisited.ShouldContain(new Point(x: -1, y: -1));
    }
    
    [Fact]
    protected void CalculatePointsVisited_EnsureSameStepCountedOnce()
    {
        // Arrange
        
        IRobot robot = new TRobot();

        robot.StartPoint = new Point
        {
            X = 0,
            Y = 0,
        };
        robot.Commands = new List<Command>()
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
        };
        
        // Act

        Point[] pointsVisited = robot
            .CalculatePointsVisited()
            .ToArray();

        // Assert

        pointsVisited.Count().ShouldBe(2);
        pointsVisited.ShouldContain(new Point(x: 0, y: 0));
        pointsVisited.ShouldContain(new Point(x: 1, y: 0));
    }
}