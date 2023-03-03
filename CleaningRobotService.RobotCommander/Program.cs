using System.Text.Json.Serialization;
using CleaningRobotService.RobotCommander;

WebApplication
    .CreateBuilder(args)
    .RegisterServices()
    .Build()
    .SetupMiddleware()
    .Run();

namespace CleaningRobotService.RobotCommander
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
                // c.DocumentFilter<LowercaseDocumentFilter>();
            });
            
            // builder.Services.AddDatabaseServices(connectionString: appConfiguration.DatabaseConnectionString);
            // builder.Services.InjectRepositories();

            // builder.Services.InjectServices();

            builder.Services
                .AddHealth(appConfiguration: appConfiguration);
        
            return builder;
        }

        private static void AddHealth(this IServiceCollection services, AppConfiguration appConfiguration)
        {
            services
                .AddHealthChecks();
        }
    }

    static class SetupMiddlewarePipeline
    {
        public static WebApplication SetupMiddleware(this WebApplication app)
        {
            // app.RunEfMigrations();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}