using CleaningRobotService.Web.Services;
using CleaningRobotService.Web.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CleaningRobotService.Web.Tests;

public class TestEnvironment
{
    private readonly DbContextOptions<ServiceDbContext> DbContextOptions;
    private readonly AppConfiguration AppConfiguration;
    private readonly DatabaseFixture DatabaseFixture;
    
    public TestEnvironment(DatabaseFixture databaseFixture)
    {
        this.DatabaseFixture = databaseFixture;
    }
    
    public CommandRobotService GetCommandRobotService(ServiceDbContext? context = null)
    {
        context ??= this.DatabaseFixture.CreateContext();

        return new CommandRobotService(context: context);
    }
}