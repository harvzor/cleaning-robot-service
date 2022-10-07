using CleaningRobotService.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CleaningRobotService.Web;

public class ServiceDbContext : DbContext
{
    public ServiceDbContext()
    {
    }
    
    /// <summary>
    /// Constructor required for EF migrations.
    /// </summary>
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
    {
    }
    
    public virtual DbSet<Execution> Executions => Set<Execution>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseNpgsql()
            .UseCamelCaseNamingConvention();
}
