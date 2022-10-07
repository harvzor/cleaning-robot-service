using CleaningRobotService.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CleaningRobotService.Web;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public virtual DbSet<Execution> Executions { get; set; }
}
