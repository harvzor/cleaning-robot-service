using CleaningRobotService.Web;
using CleaningRobotService.Web.Filters;
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

SetupDependencyInjection();

builder.Services.AddSwaggerGen(c =>
{
    c.DocumentFilter<LowercaseDocumentFilter>();
});
//builder.Services.AddJsonOptions(options => 
//    options.JsonSerializerOptionsons.Converters.Add(new JsonStringEnumConverter()));
// builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
// {
//     options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
//     options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
// });

builder.Services.AddDbContext<ServiceDbContext>(options =>
    options
        .UseLazyLoadingProxies(true)
        .UseNpgsql(appConfiguration.DatabaseConnectionString)
);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

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

void SetupDependencyInjection()
{
    // System.Text.Json.JsonSerializerOptions jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions();
    // jsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    // builder.Services.AddSingleton<System.Text.Json.JsonSerializerOptions>(jsonSerializerOptions);

    // Microsoft.AspNetCore.Http.Json.JsonOptions jsonOptions = new();
    // jsonOptions.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    // builder.Services.AddSingleton(jsonOptions);
}

void RunEfMigrations()
{
    using var scope = app.Services.CreateScope();
    var database = scope.ServiceProvider.GetRequiredService<ServiceDbContext>().Database;

    database.SetCommandTimeout(TimeSpan.FromHours(6));

    app.Logger.LogInformation("Running database migrations.");
    database.Migrate();
    app.Logger.LogInformation("Finished running database migrations.");
}
