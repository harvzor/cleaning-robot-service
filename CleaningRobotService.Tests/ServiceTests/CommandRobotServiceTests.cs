using System.Collections.ObjectModel;
using System.Drawing;
using CleaningRobotService.BusinessLogic.Services;
using CleaningRobotService.Common.Dtos.Input;
using CleaningRobotService.Common.Enums;
using CleaningRobotService.Common.Helpers;
using CleaningRobotService.DataPersistence;
using CleaningRobotService.DataPersistence.Models;
using CleaningRobotService.Tests.Fixtures;
using Shouldly;
using Xunit;

namespace CleaningRobotService.Tests.ServiceTests;

[Collection(nameof(DefaultFixtureCollection))]
public class CommandRobotServiceTests
{
    private readonly CommandRobotService _commandRobotService;
    private readonly DatabaseFixture _databaseFixture;

    public CommandRobotServiceTests(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
        
        TestEnvironment testEnvironment = new TestEnvironment(databaseFixture: _databaseFixture);
        
        _commandRobotService = testEnvironment.GetCommandRobotService();
    }

    /// <summary>
    /// Test that <see cref="Execution"/>s are stored in the db.
    /// </summary>
    [Fact]
    public void CreateCommandRobot()
    {
        // Arrange.

        ReadOnlyCollection<CommandDto> commands = new List<CommandDto>
        {
            new()
            {
                Direction = DirectionEnum.East,
                Steps = 1,
            },
        }
        .AsReadOnly();
        
        // Freeze time so I can test it later.
        SystemDateTime.SetConstant();
        
        // Act
        
        Execution execution = _commandRobotService
            .CreateCommandRobot(
                startPoint: new Point
                {
                    X = 0,
                    Y = 0,
                },
                commands: new ReadOnlyCollection<CommandDto>(commands),
                runExecutionAsync: false
            );

        // Assert

        using (ServiceDbContext context = _databaseFixture.CreateContext())
        {
            Execution? storedExecution = context.Executions.Find(execution.Id);

            storedExecution.ShouldNotBeNull();
            storedExecution.Commands.ShouldBe(1);
            storedExecution.CreatedAt.DateTime
                // Have to set the tolerance because Postgres stores DateTimes slightly less precise than .NET.
                // https://stackoverflow.com/questions/51103606/storing-datetime-in-postgresql-without-loosing-precision
                //     should be
                // 2022-10-09T16:46:50.0393992Z
                //    but was
                // 2022-10-09T16:46:50.0393990
                .ShouldBe(SystemDateTime.UtcNow, new TimeSpan(0, 0, 0, 0, 1));
            storedExecution.Result.ShouldBe(2);
            storedExecution.Duration.ShouldNotBe(null);
            storedExecution.Duration!.Value.ShouldBeGreaterThan(TimeSpan.FromTicks(1));
        }
    }
}