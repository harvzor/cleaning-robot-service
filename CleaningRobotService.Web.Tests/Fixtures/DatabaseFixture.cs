using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CleaningRobotService.Web.Tests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
// This is used in a ICollectionFixture, probably instantiated by DI.
public class DatabaseFixture : IDisposable
{
    private static readonly object Lock = new();
    private static bool _databaseInitialized;

    private readonly DbConnection _connection;
    private readonly DbContextOptions<ServiceDbContext> _dbContextOptions;

    public DatabaseFixture()
    {
        AppConfiguration appConfiguration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
            .Build()
            .GetSection("App")
            .Get<AppConfiguration>();

        _connection = new NpgsqlConnection(appConfiguration.DatabaseConnectionString);

        _dbContextOptions = new DbContextOptionsBuilder<ServiceDbContext>()
            .UseLazyLoadingProxies()
            .UseNpgsql(appConfiguration.DatabaseConnectionString)
            //.UseInMemoryDatabase(databaseName: databaseName) // Can't use in memory db because it does not support array properties (Guid[]).
            .Options;

        CreateDatabase();

        _connection.Open();
    }

    public ServiceDbContext CreateContext()
    {
        ServiceDbContext context = new(_dbContextOptions);

        return context;
    }

    public ServiceDbContext CreateContext(DbTransaction transaction)
    {
        ServiceDbContext context = new(_dbContextOptions);
        context.Database.UseTransaction(transaction);

        return context;
    }

    public DbTransaction BeginTransaction()
    {
        return _connection.BeginTransaction();
    }

    private void CreateDatabase()
    {
        lock (Lock)
        {
            if (!_databaseInitialized)
            {
                using (ServiceDbContext context = new(_dbContextOptions))
                {
                    context.Database.EnsureDeleted();
                    context.Database.Migrate();

                    context.SaveChanges();
                }

                _databaseInitialized = true;
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            _connection.Close();
    }
}