using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Enums;
using CleaningRobotService.Web.Helpers;
using CleaningRobotService.Web.Models;
using CleaningRobotService.Web.Services;
using CleaningRobotService.Web.Structs;
using CleaningRobotService.Web.Tests.Fixtures;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Web.Tests.ServiceTests;

[Collection(nameof(DefaultFixtureCollection))]
public class CommandRobotServiceTests
{
    private readonly CommandRobotService CommandRobotService;
    private readonly DatabaseFixture DatabaseFixture;
    private readonly TestEnvironment TestEnvironment;

    public CommandRobotServiceTests(DatabaseFixture databaseFixture)
    {
        this.DatabaseFixture = databaseFixture;
        this.TestEnvironment = new TestEnvironment(databaseFixture: this.DatabaseFixture);
        this.CommandRobotService = this.TestEnvironment.GetCommandRobotService();
    }
    
    [Fact]
    public void CalculateIndicesCleanedTest()
    {
        // Arrange / Act
        
        int result = CommandRobotService.CalculateIndicesVisited(
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
    
    [Fact]
    public void CalculateIndicesCleaned_EnsureSameStepCountedOnce()
    {
        // Arrange / Act
        
        int result = CommandRobotService.CalculateIndicesVisited(
            startPoint: new Point
            {
                X = 0,
                Y = 0,
            },
            commands: new List<Command>()
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
        );
        
        // Assert

        result.ShouldBe(2);
    }
    
    /// <summary>
    /// Test that <see cref="Execution"/>s are stored in the db.
    /// </summary>
    [Fact]
    public void CreateCommandRobot()
    {
        // Arrange.
        
        CommandRobotPostDto commandRobotPostDto = new()
        {
            Start = new Point
            {
                X = 0,
                Y = 0,
            },
            Commands = new List<Command>
            {
                new()
                {
                    Direction = DirectionEnum.east,
                    Steps = 1,
                },
            }
        };
        
        // Freeze time so I can test it later.
        SystemDateTime.SetConstant();
        
        // Act
        
        Execution execution = CommandRobotService.CreateCommandRobot(body: commandRobotPostDto);
        
        // Assert

        using (ServiceDbContext context = this.DatabaseFixture.CreateContext())
        {
            Execution storedExecution = context.Executions.Find(execution.Id);
            
            storedExecution.Commands.ShouldBe(1);
            storedExecution.Result.ShouldBe(2);
            storedExecution.TimeStamp.DateTime
                // Have to set the tolerance because Postgres stores DateTimes slightly less precise than .NET.
                // https://stackoverflow.com/questions/51103606/storing-datetime-in-postgresql-without-loosing-precision
                //     should be
                // 2022-10-09T16:46:50.0393992Z
                //    but was
                // 2022-10-09T16:46:50.0393990
                .ShouldBe(SystemDateTime.UtcNow, new TimeSpan(0, 0, 0, 0, 1));
            storedExecution.Duration.ShouldBeGreaterThan(0);
        }
    }
}