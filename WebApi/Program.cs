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

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors(builderConfig => builderConfig.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

    app.UseRateLimiter();
    
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
        .WithOpenApi();
    
    app.MapGet("/weatherforecast/{cityName}/{latitude}/{longitude}",
        async (string cityName, double latitude, double longitude, WeatherService weatherForecastService) =>
        {
            var forecast = await weatherForecastService
                .GetForecastAsync(cityName, latitude, longitude);
            
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

