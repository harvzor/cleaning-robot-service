using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CleaningRobotService.Web.Tests.Fixtures;

    public class DatabaseFixture : IDisposable
    {
        private static readonly object Lock = new();
        private static bool DatabaseInitialized;

        private readonly AppConfiguration AppConfiguration;
        private readonly DbConnection Connection;
        private readonly DbContextOptions<ServiceDbContext> DbContextOptions;

        public DatabaseFixture()
        {
            this.AppConfiguration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build()
                .GetSection("App")
                .Get<AppConfiguration>();

            this.Connection = new NpgsqlConnection(this.AppConfiguration.DatabaseConnectionString);

            this.DbContextOptions = new DbContextOptionsBuilder<ServiceDbContext>()
                .UseLazyLoadingProxies(true)
                .UseNpgsql(this.AppConfiguration.DatabaseConnectionString)
                //.UseInMemoryDatabase(databaseName: databaseName) // Can't use in memory db because it does not support array properties (Guid[]).
                .Options;

            this.CreateDatabase();

            this.Connection.Open();
        }

        public ServiceDbContext CreateContext()
        {
            ServiceDbContext context = new(DbContextOptions);

            return context;
        }

        public ServiceDbContext CreateContext(DbTransaction transaction)
        {
            ServiceDbContext context = new(DbContextOptions);
            context.Database.UseTransaction(transaction);

            return context;
        }

        public DbTransaction BeginTransaction()
        {
            return Connection.BeginTransaction();
        }

        private void CreateDatabase()
        {
            lock (Lock)
            {
                if (!DatabaseInitialized)
                {
                    using (ServiceDbContext context = new(this.DbContextOptions))
                    {
                        context.Database.EnsureDeleted();
                        context.Database.Migrate();

                        context.SaveChanges();
                    }

                    DatabaseInitialized = true;
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
                Connection.Close();
        }
    }