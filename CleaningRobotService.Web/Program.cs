using System.Text.Json.Serialization;
using CleaningRobotService.DataPersistence;
using CleaningRobotService.Web;
using CleaningRobotService.Web.Filters;
using Microsoft.EntityFrameworkCore;

WebApplication
    .CreateBuilder(args)
    .RegisterServices()
    .Build()
    .SetupMiddleware()
    .Run();

namespace CleaningRobotService.Web
{
    static class SetupServices
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions<AppConfiguration>()
                .BindConfiguration("App")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            AppConfiguration appConfiguration = builder.Configuration
                .GetSection("App")
                .Get<AppConfiguration>();

            // Add services to the container.

            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<LowercaseDocumentFilter>();
            });

            builder.Services.AddDbContext<ServiceDbContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .UseNpgsql(appConfiguration.DatabaseConnectionString)
            );
        
            return builder;
        }
    }

    static class SetupMiddlewarePipeline
    {
        public static WebApplication SetupMiddleware(this WebApplication app)
        {
            app.RunEfMigrations();
        
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        
            return app;
        }
    
        private static void RunEfMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<ServiceDbContext>().Database;

            database.SetCommandTimeout(TimeSpan.FromHours(6));

            app.Logger.LogInformation("Running database migrations.");
            database.Migrate();
            app.Logger.LogInformation("Finished running database migrations.");
        }
    }
}