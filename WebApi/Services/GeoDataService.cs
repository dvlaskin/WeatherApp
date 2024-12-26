using System.Text.Json;
using WebApi.Models.OpenWeatherMap;

namespace WebApi.Services;


public interface IGeoDataService
{
    Task<IEnumerable<CityCoordinate>> GetCitiesCoordinateAsync(
        string cityName, CancellationToken cancellationToken = default
    );
}

public class GeoDataService : IGeoDataService
{
    private readonly string cacheKey = "geodata:{0}";
    private readonly ILogger<GeoDataService> logger;
    private readonly ICacheService cacheService;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly string apiKey;


    public GeoDataService(
        ILogger<GeoDataService> logger, 
        ICacheService cacheService,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration
    )
    {
        this.logger = logger;
        this.cacheService = cacheService;
        this.httpClientFactory = httpClientFactory;
        this.apiKey = configuration["ApiKeys:OpenWeatherMapApiKey"] ?? string.Empty;
    }


    public async Task<IEnumerable<CityCoordinate>> GetCitiesCoordinateAsync(string cityName, CancellationToken cancellationToken = default)
    {
        var cityCacheKey = string.Format(cacheKey, cityName);
        var cachedGeoData = await cacheService.GetAsync<List<CityCoordinate>>(cityCacheKey);

        if (cachedGeoData is not null)
        {
            logger.LogInformation("Found cache geo data for {City}", cityName);
            return cachedGeoData;
        }
        
        var httpClient = httpClientFactory.CreateClient(AppConstants.OpenWeatherMapHttpClient);
        var urlString = $"/geo/1.0/direct?q={cityName}&limit=10&appid={apiKey}";
        var response = await httpClient.GetAsync(urlString, cancellationToken);

        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        
        var result = string.IsNullOrEmpty(responseString) 
            ? new List<CityCoordinate>() 
            : JsonSerializer.Deserialize<IEnumerable<CityCoordinate>>(responseString)!.ToList();
        
        // group by country and state
        result = result.GroupBy(x => new { x.State, x.Country })
            .Select(x => x.First())
            .ToList();
        
        logger.LogInformation("Requested geo data for {City}", cityName);

        await cacheService.SetAsync(cityCacheKey, result);
        
        return result;
    }
}