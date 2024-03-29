using CleaningRobotService.BusinessLogic.Services;
using CleaningRobotService.DataPersistence;
using CleaningRobotService.DataPersistence.Repositories;
using CleaningRobotService.Tests.Fixtures;

namespace CleaningRobotService.Tests;

public class TestEnvironment
{
    private readonly DatabaseFixture _databaseFixture;
    
    public TestEnvironment(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }
    
    public CommandRobotService GetCommandRobotService(ServiceDbContext? context = null)
    {
        context ??= _databaseFixture.CreateContext();

        return new CommandRobotService(
            commandRepository: new CommandRepository(context),
            executionRepository: new ExecutionRepository(context)
        );
    }
}