using System.Text.Json;
using Edison.Abstractions;
using StackExchange.Redis;

namespace Edison.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _database;
        private readonly ISubscriber _subscriber;

        public RedisService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
            _subscriber = redis.GetSubscriber();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, json, expiry);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;
            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<bool> PublishAsync(string channel, string message)
        {
            var result = await _subscriber.PublishAsync(RedisChannel.Literal(channel), message);
            return result > 0;
        }

        public async Task SubscribeAsync(string channel, Action<RedisChannel, RedisValue> handler)
        {
            await _subscriber.SubscribeAsync(RedisChannel.Literal(channel), handler);
        }
    }
}