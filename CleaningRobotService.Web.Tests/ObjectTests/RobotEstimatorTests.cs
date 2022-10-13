using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Interfaces;
using CleaningRobotService.Web.Objects;
using CleaningRobotService.Web.Structs;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Web.Tests.ObjectTests;

public class RobotEstimatorTests
{
    [Fact]
    public void CalculatePointsVisitedTest()
    {
        // Arrange

        RobotEstimator robot = new()
        {
            StartPoint = new Point
            {
                X = 10,
                Y = 22,
            },
            Commands = new List<Command>()
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
            }
        };

        // Act

        int numberOfPointsVisited = robot
            .CalculateNumberOfPointsVisited();
        
        // Assert

        numberOfPointsVisited.ShouldBe(4);
    }
    
    [Fact]
    public void CalculatePointsVisitedTest_NoCommands()
    {
        // Arrange

        RobotEstimator robot = new()
        {
            StartPoint = new Point
            {
                X = 0,
                Y = 0,
            },
            Commands = new List<Command>(),
        };

        // Act

        int numberOfPointsVisited = robot
            .CalculateNumberOfPointsVisited();
        
        // Assert

        numberOfPointsVisited.ShouldBe(1);
    }
    
    [Fact]
    public void CalculatePointsVisitedTest_Square()
    {
        // Arrange

        RobotEstimator robot = new()
        {
            StartPoint = new Point
            {
                X = 0,
                Y = 0,
            },
            Commands = new List<Command>()
            {
                new Command
                {
                    Direction = DirectionEnum.north,
                    Steps = 1,
                },
                new Command
                {
                    Direction = DirectionEnum.east,
                    Steps = 1,
                },
                new Command
                {
                    Direction = DirectionEnum.south,
                    Steps = 1,
                },
                // Duplicate point.
                new Command
                {
                    Direction = DirectionEnum.west,
                    Steps = 1,
                },
            }
        };

        // Act

        int numberOfPointsVisited = robot
            .CalculateNumberOfPointsVisited();
        
        // Assert

        numberOfPointsVisited.ShouldBe(4);
    }
    
    [Fact]
    public void CalculatePointsVisited_EnsureSameStepCountedOnce()
    {
        // Arrange

        RobotEstimator robot = new()
        {
            StartPoint = new Point
            {
                X = 0,
                Y = 0,
            },
            Commands = new List<Command>()
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
            }
        };

        // Act

        int numberOfPointsVisited = robot.CalculateNumberOfPointsVisited();

        // Assert

        numberOfPointsVisited.ShouldBe(2);
    }
    
    [Fact]
    public void CalculatePointsVisitedTest_LoopOffset1()
    {
        // Arrange

        RobotEstimator robot = new()
        {
            StartPoint = new Point
            {
                X = 0,
                Y = 0,
            },
        };

        List<Command> commands = new();
        for (int i = 0; i < 2; i++)
        {
            commands.AddRange(
                new List<Command>()
                {
                    new Command
                    {
                        Direction = DirectionEnum.north,
                        Steps = 2,
                    },
                    new Command
                    {
                        Direction = DirectionEnum.east,
                        Steps = 2,
                    },
                    new Command
                    {
                        Direction = DirectionEnum.south,
                        Steps = 2,
                    },
                    new Command
                    {
                        Direction = DirectionEnum.west,
                        Steps = 1,
                    },
                }
            );
        }

        robot.Commands = commands;
        
        // Act

        int numberOfPointsVisited = robot.CalculateNumberOfPointsVisited();
        
        // Assert

        numberOfPointsVisited.ShouldBe(12);
    }
    
    [Fact]
    public void CalculatePointsVisitedTest_Loop()
    {
        // Arrange

        RobotEstimator robot = new()
        {
            StartPoint = new Point
            {
                X = 0,
                Y = 0,
            }
        };

        List<Command> commands = new();

        uint steps = 1000;

        int directionInt = 0;
        for (int i = 0; i < 1000; i++)
        {
            DirectionEnum direction = (DirectionEnum)directionInt;
            directionInt++;

            if (directionInt == 4)
                directionInt = 0;

            commands.Add(new Command
            {
                Direction = direction,
                Steps = steps,
            });
        }

        robot.Commands = commands;
        
        // Act

        int numberOfPointsVisited = robot.CalculateNumberOfPointsVisited();
        
        // Assert

        numberOfPointsVisited.ShouldBe(4000);
    }
}