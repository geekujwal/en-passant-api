using StackExchange.Redis;

namespace Edison.Abstractions
{
    public interface IRedisService
    {
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T> GetAsync<T>(string key);
        Task<bool> ExistsAsync(string key);
        Task<bool> RemoveAsync(string key);
        Task<bool> PublishAsync(string channel, string message);
        Task SubscribeAsync(string channel, Action<RedisChannel, RedisValue> handler);
    }
}