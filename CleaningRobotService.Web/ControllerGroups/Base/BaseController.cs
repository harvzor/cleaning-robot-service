using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Web.ControllerGroups.Base;

[ApiController]
//[Authorize] // May want to setup authentication of some kind before releasing to production.
[Route("robots/[controller]")]
public abstract class BaseController : Controller
{
}
