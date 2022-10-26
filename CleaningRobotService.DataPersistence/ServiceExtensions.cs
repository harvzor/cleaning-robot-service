using CleaningRobotService.DataPersistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleaningRobotService.DataPersistence;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ServiceDbContext>(options =>
            options
                .UseLazyLoadingProxies()
                .UseNpgsql(connectionString)
        );
        
        return services;
    }
    
    public static IServiceCollection InjectRepositories(this IServiceCollection services)
    {
        // TODO: maybe there's some fancy way to auto inject these so they don't have to be manually added.
        services.AddScoped<IExecutionRepository, ExecutionRepository>();
        services.AddScoped<ICommandRobotRepository, CommandRobotRepository>();
        
        return services;
    }
}
