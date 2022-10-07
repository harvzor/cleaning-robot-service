namespace CleaningRobotService.Web.Services;

public abstract class BaseService
{
    protected readonly DbContext Context;
    
    protected BaseService(DbContext context)
    {
        this.Context = context;
    }
}