using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Robots;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Tests.RobotTests;

public abstract class BaseRobotTests<TRobot> where TRobot : IRobot
{
    private IRobot CreateRobot(Point startPoint, IEnumerable<DirectionStep> commands)
    {
        return (TRobot)Activator.CreateInstance(typeof(TRobot), startPoint, commands)!;
    }
    
    [Fact]
    protected void CalculatePointsVisitedTest()
    {
        // Arrange

        IRobot robot = CreateRobot(
            startPoint: new Point
            {
                X = 10,
                Y = 22,
            },
            commands: new List<DirectionStep>()
            {
                new DirectionStep
                {
                    Direction = DirectionEnum.East,
                    Steps = 2,
                },
                new DirectionStep
                {
                    Direction = DirectionEnum.North,
                    Steps = 1,
                },
            }
        );
        
        // Act
        
        robot.CalculatePointsVisited();

        Point[] pointsVisited = robot
            .GetPointsVisited()
            .ToArray();
        
        // Assert

        robot.CountPointsVisited().ShouldBe(pointsVisited.Length);
        pointsVisited.Length.ShouldBe(4);
        pointsVisited.ShouldContain(new Point(x: 10, y: 22));
        pointsVisited.ShouldContain(new Point(x: 11, y: 22));
        pointsVisited.ShouldContain(new Point(x: 12, y: 22));
        pointsVisited.ShouldContain(new Point(x: 12, y: 23));
    }
    
    [Fact]
    protected void CalculatePointsVisited_SupportsNegativePoints()
    {
        // Arrange
        
        IRobot robot = CreateRobot(
            startPoint: new Point
            {
                X = 0,
                Y = 0,
            },
            commands: new List<DirectionStep>()
            {
                new DirectionStep
                {
                    Direction = DirectionEnum.West,
                    Steps = 1,
                },
                // Go back on itself.
                new DirectionStep
                {
                    Direction = DirectionEnum.South,
                    Steps = 1,
                },
            }
        );
        
        // Act
        
        robot.CalculatePointsVisited();

        Point[] pointsVisited = robot
            .GetPointsVisited()
            .ToArray();

        // Assert

        robot.CountPointsVisited().ShouldBe(pointsVisited.Length);
        pointsVisited.Length.ShouldBe(3);
        pointsVisited.ShouldContain(new Point(x: 0, y: 0));
        pointsVisited.ShouldContain(new Point(x: -1, y: 0));
        pointsVisited.ShouldContain(new Point(x: -1, y: -1));
    }
    
    [Fact]
    protected void CalculatePointsVisited_EnsureSameStepCountedOnce()
    {
        // Arrange
        
        IRobot robot = CreateRobot(
            startPoint: new Point
            {
                X = 0,
                Y = 0,
            },
            commands: new List<DirectionStep>()
            {
                new DirectionStep
                {
                    Direction = DirectionEnum.East,
                    Steps = 1,
                },
                // Go back on itself.
                new DirectionStep
                {
                    Direction = DirectionEnum.West,
                    Steps = 1,
                },
            }
        );
        
        // Act
        
        robot.CalculatePointsVisited();

        Point[] pointsVisited = robot
            .GetPointsVisited()
            .ToArray();
        
        // Assert

        robot.CountPointsVisited().ShouldBe(pointsVisited.Length);
        pointsVisited.Length.ShouldBe(2);
        pointsVisited.ShouldContain(new Point(x: 0, y: 0));
        pointsVisited.ShouldContain(new Point(x: 1, y: 0));
    }
    
    [Fact]
    protected void CalculatePointsVisitedTest_Loop()
    {
        // Arrange

        List<DirectionStep> commands = CommandGenerator.LoopCommands(steps: 1000);
        
        IRobot robot = CreateRobot(
            startPoint: new Point
            {
                X = 0,
                Y = 0,
            },
            commands: commands
        );
        
        // Act
        
        robot.CalculatePointsVisited();

        Point[] pointsVisited = robot
            .GetPointsVisited()
            .ToArray();
        
        // Assert

        robot.CountPointsVisited().ShouldBe(pointsVisited.Length);
        pointsVisited.Length.ShouldBe(4000);
        // pointsVisited[0].ShouldBe(new Point(x: 0, y: 0)); // Start.
        // pointsVisited[1].ShouldBe(new Point(x: 0, y: (int)steps)); // North.
        // pointsVisited[2].ShouldBe(new Point(x: (int)steps, y: (int)steps)); // East
        // pointsVisited[3].ShouldBe(new Point(x: (int)steps, y: 0)); // South
    }
    
    [Fact]
    protected void CalculatePointsVisitedTest_SpiralIn()
    {
        // Arrange

        const int width = 10;
        List<DirectionStep> commands = CommandGenerator.SpiralIn(width: width);
                
        IRobot robot = CreateRobot(
            startPoint: new Point
            {
                X = 0,
                Y = 0,
            },
            commands: commands
        );
        
        // Act
        
        robot.CalculatePointsVisited();

        Point[] pointsVisited = robot
            .GetPointsVisited()
            .ToArray();
        
        // Assert

        robot.CountPointsVisited().ShouldBe(pointsVisited.Length);
        pointsVisited.Length.ShouldBe(width * width);
    }
}