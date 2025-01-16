using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApi;
using WebApi.IoC;
using WebApi.Services;

var startupLogger = LoggerControl.CreateStartupLogger();
startupLogger.LogInformation("Web API starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    // load config values from env file
    DotNetEnv.Env.Load("../.env");
    
    // setup config
    builder.Configuration.AddConfig();

    // setup logger
    builder.Logging.AddLogger();
    
    // add health checks
    builder.Services.AddAppHealthChecks();
    
    // add rate limiter
    builder.Services.AddApiRateLimiter();

    // add services to the container.
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddCors();

    // add cache
    builder.AddRedisClient(connectionName: "cache");

    // add services
    builder.Services.AddApplicationServices();
    
    // add http factories
    builder.Services.AddHttpClients(builder.Configuration);
    
    // add open telemetry 
    builder.Services.AddOpenTelemetry(builder.Configuration);

    var app = builder.Build();

    var healthReport = await app.Services.GetRequiredService<HealthCheckService>().CheckHealthAsync();
    if (healthReport.Status == HealthStatus.Unhealthy)
    {
        startupLogger.LogCritical(
            "API health check is unhealthy: {Message}", 
            healthReport.Entries
                .Select(s => new { s.Key, s.Value.Status })
                .ToList()
        );

        await app.StopAsync();
        return;
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors(builderConfig => builderConfig.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

    app.UseRateLimiter();
    
    app.MapHealthChecks("/health")
        .WithName("HealthCheck")
        .WithOpenApi()
        .RequireRateLimiting(AppConstants.SlidingWindowLimiter);
    
    app.MapGet("/city/{cityName}", async (string cityName, IGeoDataService geoDataService) =>
        {
            var result = await geoDataService.GetCitiesCoordinateAsync(cityName);
            
            return result;
        })
        .WithName("GetCity")
        .WithOpenApi()
        .RequireRateLimiting(AppConstants.SlidingWindowLimiter);
    
    app.MapGet("/currentweather/{cityName}/{latitude}/{longitude}", 
        async (string cityName, double latitude, double longitude, WeatherService weatherForecastService) =>
        {
            var forecast = await weatherForecastService
                .GetCurrentWeatherAsync(cityName, latitude, longitude);
            
            return forecast;
        })
        .WithName("GetCurrentWeather")
        .WithOpenApi()
        .RequireRateLimiting(AppConstants.SlidingWindowLimiter);
    
    app.MapGet("/weatherforecast/{cityName}/{latitude}/{longitude}",
        async (string cityName, double latitude, double longitude, WeatherService weatherForecastService) =>
        {
            var forecast = await weatherForecastService
                .GetForecastAsync(cityName, latitude, longitude);
            
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi()
        .RequireRateLimiting(AppConstants.SlidingWindowLimiter);
    
    
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

