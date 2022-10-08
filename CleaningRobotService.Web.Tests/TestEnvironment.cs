using CleaningRobotService.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CleaningRobotService.Web.Tests;

public class TestEnvironment
{
    private readonly DbContextOptions<ServiceDbContext> DbContextOptions;
    private readonly AppConfiguration AppConfiguration;
    
    public TestEnvironment()
    {
        this.AppConfiguration = this.LoadConfiguration();

        this.DbContextOptions = new DbContextOptionsBuilder<ServiceDbContext>()
            .UseLazyLoadingProxies(true)
            .UseNpgsql(this.AppConfiguration.DatabaseConnectionString)
            //.UseInMemoryDatabase(databaseName: databaseName) // Can't use in memory db because it does not support array properties (Guid[]).
            .Options;
    }
    
    private ServiceDbContext CreateContext() => new(this.DbContextOptions);
    
    private AppConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
            .Build()
            .GetSection("App")
            .Get<AppConfiguration>();
    }
    
    public CommandRobotService GetCommandRobotService(ServiceDbContext? context = null)
    {
        context ??= this.CreateContext();

        return new CommandRobotService(context: context);
    }
}