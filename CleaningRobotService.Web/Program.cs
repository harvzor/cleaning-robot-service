using CleaningRobotService.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOptions<AppConfiguration>()
    .BindConfiguration("App")
    .ValidateDataAnnotations()
    .ValidateOnStart();

AppConfiguration appConfiguration = builder.Configuration.GetSection("App").Get<AppConfiguration>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContext>(options =>
    options
        .UseLazyLoadingProxies(true)
        .UseNpgsql(appConfiguration.DatabaseConnectionString)
);

var app = builder.Build();

RunEfMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void RunEfMigrations()
{
    var logger = app.Services.GetRequiredService<ILogger>();
    var database =
        app.Services.GetRequiredService<ServiceDbContext>().Database;

    database.SetCommandTimeout(TimeSpan.FromHours(6));

    logger.LogInformation("Running database migrations.");
    database.Migrate();
    logger.LogInformation("Finished running database migrations.");
}
