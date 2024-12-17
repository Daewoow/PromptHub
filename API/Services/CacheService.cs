using System.Text.Json;
using StackExchange.Redis;

namespace WebApplication1.Cache;

public class CacheService(IConnectionMultiplexer multiplexer)
{
    private readonly IDatabase _database = multiplexer.GetDatabase();

    public async Task<T> GetOrAdd<T>(string key, Func<Task<T>> itemFactory, int expiration)
    {
        var excitingItem = await _database.StringGetAsync(key);
        if (excitingItem.HasValue)
            return JsonSerializer.Deserialize<T>(excitingItem);

        var newItem = await itemFactory();

        await _database.StringSetAsync(key, 
            JsonSerializer.Serialize(newItem), TimeSpan.FromSeconds(expiration));

        return newItem;
    }

    public async Task Invalidate(string key)
        => await _database.KeyDeleteAsync(key);
}