using CleaningRobotService.Tests.Fixtures;
using CleaningRobotService.Web;
using CleaningRobotService.Web.Services;

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

        return new CommandRobotService(context: context);
    }
}