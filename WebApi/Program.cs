using WebApi.IoC;
using WebApi.Services;

var startupLogger = LoggerControl.CreateStartupLogger();
startupLogger.LogInformation("Web API starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // setup logger
    builder.Logging.AddLogger();

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // add cache
    builder.AddRedisClient(connectionName: "cache");

    // add services
    builder.Services.AddTransient<WeatherForecastService>();

    // add open telemetry 
    builder.Services.AddOpenTelemetry(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();


    app.MapGet("/weatherforecast", async (WeatherForecastService weatherForecastService) =>
        {
            var forecast = await weatherForecastService.GetForecastAsync("New York");
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();
    
    app.Run();
}
catch (Exception ex)
{
    startupLogger.LogCritical(ex, "Web API starting failed: {ErrorMessage}", ex.Message);
}
finally
{
    startupLogger.LogInformation("Web API stopped.");
}

