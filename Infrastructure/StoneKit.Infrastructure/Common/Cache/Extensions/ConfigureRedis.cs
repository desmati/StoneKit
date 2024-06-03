using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

namespace System.Caching
{
    internal static class RedisExtensions
    {
        private const string PATH_RedisOptions = @"Assets\Settings\redis.json";

        internal static void ConfigureRedis(this WebApplicationBuilder builder, string? appName)
        {
            if (!File.Exists(PATH_RedisOptions) || string.IsNullOrEmpty(appName))
            {
                return;
            }

            builder.Configuration.AddJsonFile(PATH_RedisOptions, false, true);

            var redisSection = builder.Configuration.GetSection("redis");
            if (redisSection == null)
            {
                return;
            }

            builder.Services.Configure<RedisSection>(redisSection);

            var redisOptions = redisSection.Get<RedisSection>();
            if (string.IsNullOrEmpty(redisOptions?.Config))
            {
                return;
            }

            builder.Services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = redisOptions.Config;
                option.InstanceName = appName.Replace(" ", "_").ToLower();
            });

            var redisInstance = ConnectionMultiplexer.Connect(redisOptions.Config);
            if (redisInstance == null)
            {
                return;
            }

            builder.Services.AddSingleton<IConnectionMultiplexer>(redisInstance);
            builder.Services.AddTransient<IStaticCache, StaticCache>();
        }
    }
}
