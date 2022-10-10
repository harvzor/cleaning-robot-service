namespace CleaningRobotService.Web.Services;

public abstract class BaseService
{
    protected readonly ServiceDbContext Context;
    
    protected BaseService(ServiceDbContext context)
    {
        Context = context;
    }
}