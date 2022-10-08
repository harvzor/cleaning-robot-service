using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Services;
using CleaningRobotService.Web.Structs;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Web.Tests.ServiceTests;

public class CommandRobotServiceTests
{
    private readonly CommandRobotService CommandRobotService;

    public CommandRobotServiceTests()
    {
        TestEnvironment testEnvironment = new();
        this.CommandRobotService = testEnvironment.GetCommandRobotService();
    }
    
    [Fact]
    public void CalculateIndicesCleanedTest()
    {
        // Arrange / Act
        
        int result = this.CommandRobotService.CalculateIndicesCleaned(
            startPoint: new Point
            {
                X = 10,
                Y = 22,
            },
            commands: new List<Command>()
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
        );
        
        // Assert

        result.ShouldBe(4);
    }
}