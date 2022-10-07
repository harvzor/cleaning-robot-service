using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.Controllers;

[ApiController]
//[Authorize] // May want to setup authentication of some kind before releasing to production.
[Route("[controller]")]
public abstract class BaseController : Controller
{
    protected readonly DbContext Context;
    
    protected BaseController(DbContext context)
    {
        this.Context = context;
    }
}