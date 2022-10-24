using System.Drawing;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Robots;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Tests.RobotTests;

public abstract class BaseRobotTests<TRobot> where TRobot : IRobot
{
    private IRobot CreateRobot(Point startPoint, IEnumerable<CommandDto> commands)
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
            commands: new List<CommandDto>()
            {
                new CommandDto
                {
                    Direction = DirectionEnum.east,
                    Steps = 2,
                },
                new CommandDto
                {
                    Direction = DirectionEnum.north,
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
            commands: new List<CommandDto>()
            {
                new CommandDto
                {
                    Direction = DirectionEnum.west,
                    Steps = 1,
                },
                // Go back on itself.
                new CommandDto
                {
                    Direction = DirectionEnum.south,
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
            commands: new List<CommandDto>()
            {
                new CommandDto
                {
                    Direction = DirectionEnum.east,
                    Steps = 1,
                },
                // Go back on itself.
                new CommandDto
                {
                    Direction = DirectionEnum.west,
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
        
        List<CommandDto> commands = new();

        uint steps = 1000;

        int directionInt = 0;
        for (int i = 0; i < 1000; i++)
        {
            DirectionEnum direction = (DirectionEnum)directionInt;
            directionInt++;

            if (directionInt == 4)
                directionInt = 0;

            commands.Add(new CommandDto
            {
                Direction = direction,
                Steps = steps,
            });
        }
        
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

        List<CommandDto> commands = new();
        int width = 10;
        
        // [4][4][4][4][4]
        // [4][3][2][2][4]
        // [4][3][1][2][4]
        // [4][3][1][2][4]
        // [x][3][3][3][4]
        
        // dir   s i d doc
        // ---------------
        // north 4 0 0  0
        // east  4 1 1  1
        // south 4 2 2  1
        // west  3 3 3  2
        // north 3 4 0  2
        // east  2 5 1  3
        // south 2 6 2  3
        // west  1 7 3  4
        // north 1 8 0  4

        int directionInt = 0;
        int directionOddCount = 0;
        for (int i = 0; i < width * 2; i++)
        {
            if (directionInt == 1 || directionInt == 3)
                directionOddCount++;
            
            DirectionEnum direction = (DirectionEnum)directionInt;
            directionInt++;

            if (directionInt == 4)
                directionInt = 0;

            commands.Add(new CommandDto
            {
                Direction = direction,
                Steps = i == 0
                    ? (uint)(width - 1)
                    : (uint)(width - directionOddCount),
            });
        }
                
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