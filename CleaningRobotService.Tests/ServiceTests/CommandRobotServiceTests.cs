using System.Drawing;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Helpers;
using CleaningRobotService.DataPersistence;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.Tests.Fixtures;
using CleaningRobotService.Web.Dtos.Input;
using CleaningRobotService.Web.Services;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Tests.ServiceTests;

[Collection(nameof(DefaultFixtureCollection))]
public class CommandRobotServiceTests
{
    private readonly CommandRobotService _commandRobotService;
    private readonly DatabaseFixture _databaseFixture;
    private readonly TestEnvironment _testEnvironment;

    public CommandRobotServiceTests(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
        _testEnvironment = new TestEnvironment(databaseFixture: _databaseFixture);
        _commandRobotService = _testEnvironment.GetCommandRobotService();
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
            Commands = new List<WebCommandDto>
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
        
        Execution execution = _commandRobotService.CreateCommandRobot(body: commandRobotPostDto);
        
        // Assert

        using (ServiceDbContext context = _databaseFixture.CreateContext())
        {
            Execution? storedExecution = context.Executions.Find(execution.Id);

            storedExecution.ShouldNotBeNull();
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