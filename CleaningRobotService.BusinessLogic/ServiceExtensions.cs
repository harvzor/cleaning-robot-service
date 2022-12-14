using CleaningRobotService.BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleaningRobotService.BusinessLogic;

public static class ServiceExtensions
{
    public static IServiceCollection InjectServices(this IServiceCollection services)
    {
        // TODO: maybe there's some fancy way to auto inject these so they don't have to be manually added.
        services.AddScoped<ICommandRobotService, CommandRobotService>();
        
        return services;
    }
}
