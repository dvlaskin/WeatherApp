using WebApi;
using WebApi.IoC;
using WebApi.Services;
using WebApi.Services.CurrentWeather;
using WebApi.Services.Forecast;

var startupLogger = LoggerControl.CreateStartupLogger();
startupLogger.LogInformation("Web API starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    // load config values from env file
    DotNetEnv.Env.Load("../.env");
    
    // setup config
    builder.Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    // setup logger
    builder.Logging.AddLogger();

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddCors();

    // add cache
    builder.AddRedisClient(connectionName: "cache");

    // add services
    builder.Services.AddSingleton<ICacheService, RedisService>();
    builder.Services.AddScoped<ICurrentWeatherService, OpenMeteoCurrentWeather>();
    builder.Services.AddScoped<ICurrentWeatherCollector, CurrentWeatherCollector>();
    builder.Services.AddScoped<IForecastService, OpenMeteoForecastService>();
    builder.Services.AddScoped<IForecastCollector, ForecastCollector>();
    builder.Services.AddScoped<IGeoDataService, GeoDataService>();
    builder.Services.AddScoped<WeatherService>();
    
    
    // add http factories
    builder.Services.AddHttpClient(
        AppConstants.OpenWeatherMapHttpClient,
        client =>
        {
            client.BaseAddress = new Uri(
                builder.Configuration.GetSection("OpenWeatherMap:BaseUrl").Value ?? string.Empty
            );
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
    );
    
    
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
    
    app.MapGet("/city/{cityName}", async (string cityName, IGeoDataService geoDataService) =>
        {
            var result = await geoDataService.GetCitiesCoordinateAsync(cityName);
            
            return result;
        })
        .WithName("GetCity")
        .WithOpenApi();
    
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

