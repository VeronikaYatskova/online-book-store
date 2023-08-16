using System.Text.Json;
using Comments.DAL.Entities;
using Comments.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Comments.DAL.Repositories.Implementations
{
    public class CacheRepository : ICacheRepository
    {
        private readonly CacheSettings _cacheSettings;
        private readonly IDatabase _cacheDb;

        public CacheRepository(IOptions<CacheSettings> cacheSettings, IDatabase cacheDb)
        {
            _cacheSettings = cacheSettings.Value;

            var redis = ConnectionMultiplexer.Connect(_cacheSettings.ConnectionPort);
            _cacheDb = redis.GetDatabase();
        }

        public async Task<T?> GetDataAsync<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);

            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
    
            return JsonSerializer.Deserialize<T>(value!);
        }
        
        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            
            return await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
        }

        public async Task<object> RemoveDataAsync(string key)
        {
            var isExist = await _cacheDb.KeyExistsAsync(key);

            if (!isExist)
            {
                return false;
            }

            return await _cacheDb.KeyDeleteAsync(key);
        }
    }
}
