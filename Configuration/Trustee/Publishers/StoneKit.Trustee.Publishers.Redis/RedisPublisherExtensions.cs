namespace Trustee.Publishers;

using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

using System;

using Trustee.Host;


/// <summary>
/// Provides extension methods to register Redis as a configuration publisher.
/// </summary>
public static class RedisPublisherExtensions
{
    /// <summary>
    /// Adds Redis as the configuration publisher.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationServiceBuilder"/> to add services to.</param>
    /// <param name="configuration">The string configuration for the Redis multiplexer.</param>
    /// <returns>An <see cref="IConfigurationServiceBuilder"/> that can be used to further configure the 
    /// ConfigurationService services.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> or <paramref name="configuration"/> is null.</exception>
    public static IConfigurationServiceBuilder AddRedisPublisher(this IConfigurationServiceBuilder builder, string configuration)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        var options = ConfigurationOptions.Parse(configuration);

        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IPublisher, RedisPublisher>();

        return builder;
    }
}
