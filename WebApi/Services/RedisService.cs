using System.Text.Json;
using StackExchange.Redis;

namespace WebApi.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
}

public class RedisService : ICacheService
{
    private readonly IDatabase db;
    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
    
    
    public RedisService(IConnectionMultiplexer redis)
    {
        this.db = redis.GetDatabase();
    }
    
    
    public async Task<T?> GetAsync<T>(string key)
    {
        var cachedValue = await db.StringGetAsync(key.Replace(" ","_"));
        if (!cachedValue.HasValue || cachedValue.IsNullOrEmpty)
            return default;
        
        return JsonSerializer.Deserialize<T>(cachedValue!, jsonOptions);
            
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        await db.StringSetAsync(key.Replace(" ","_"), JsonSerializer.Serialize(value), expiry);
    }
}