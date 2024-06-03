using Newtonsoft.Json;

using StackExchange.Redis;

namespace System.Caching
{
    public sealed class StaticCache : IStaticCache
    {
        private readonly IDatabase _database;

        public StaticCache(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task AddAsync(string key, object value, TimeSpan duration)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var json = value == null
                ? ""
                : JsonConvert.SerializeObject(value) ?? "";

            await _database.StringSetAsync(key, json, duration);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var json = await _database.StringGetAsync(key);
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(json!);
        }

        public async Task RemoveAsync(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            await _database.KeyDeleteAsync(key);
        }

        public async Task ClearAsync()
        {
            var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints()[0]);
            foreach (var key in server.Keys())
            {
                await _database.KeyDeleteAsync(key);
            }
        }
    }

}
