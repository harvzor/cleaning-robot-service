using CleaningRobotService.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CleaningRobotService.Web;

public class ServiceDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public virtual DbSet<Execution> Executions => Set<Execution>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseNpgsql()
            .UseCamelCaseNamingConvention();
}
