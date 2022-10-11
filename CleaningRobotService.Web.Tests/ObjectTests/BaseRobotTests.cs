using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Interfaces;
using CleaningRobotService.Web.Structs;
using Shouldly;

namespace CleaningRobotService.Web.Tests.ObjectTests;

public abstract class BaseRobotTests
{
    protected void CalculatePointsVisitedTest(IRobot robot)
    {
        // Arrange

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
        pointsVisited[0].ShouldBe(new Point(x: 10, y: 22));
        pointsVisited[1].ShouldBe(new Point(x: 11, y: 22));
        pointsVisited[2].ShouldBe(new Point(x: 12, y: 22));
        pointsVisited[3].ShouldBe(new Point(x: 12, y: 23));
    }
    
    protected void CalculatePointsVisited_EnsureSameStepCountedOnce(IRobot robot)
    {
        // Arrange

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
        pointsVisited[0].ShouldBe(new Point(x: 0, y: 0));
        pointsVisited[1].ShouldBe(new Point(x: 1, y: 0));
    }
    
    protected void CalculatePointsVisitedTest_Loop(IRobot robot)
    {
        // Arrange

        robot.StartPoint = new Point
        {
            X = 0,
            Y = 0,
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

        Point[] pointsVisited = robot
            .CalculatePointsVisited()
            .ToArray();
        
        // Assert

        pointsVisited.Count().ShouldBe(4000);
        // pointsVisited[0].ShouldBe(new Point(x: 0, y: 0)); // Start.
        // pointsVisited[1].ShouldBe(new Point(x: 0, y: (int)steps)); // North.
        // pointsVisited[2].ShouldBe(new Point(x: (int)steps, y: (int)steps)); // East
        // pointsVisited[3].ShouldBe(new Point(x: (int)steps, y: 0)); // South
    }
}