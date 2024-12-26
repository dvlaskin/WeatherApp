using WebApi;
using WebApi.IoC;
using WebApi.Services;
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
    builder.Services.AddScoped<IForecastService, OpenMeteoForecastService>();
    builder.Services.AddScoped<IForecastCollector, ForecastCollector>();
    builder.Services.AddScoped<IGeoDataService, GeoDataService>();
    builder.Services.AddScoped<WeatherForecastService>();
    
    
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


    app.MapGet("/weatherforecast", async (WeatherForecastService weatherForecastService) =>
        {
            var forecast = await weatherForecastService
                .GetForecastAsync("London", 51.5073219, -0.1276474);
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();

    app.MapGet("/city/{cityName}", async (string cityName, WeatherForecastService weatherForecastService) =>
        {
            var result = await weatherForecastService.GetCoordinatesAsync(cityName);
            return result;
        })
        .WithName("GetCity")
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

