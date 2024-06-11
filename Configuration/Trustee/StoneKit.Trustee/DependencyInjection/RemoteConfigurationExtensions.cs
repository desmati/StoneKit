namespace Trustee.Client;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Security.Cryptography;


/// <summary>
/// Provides extension methods for adding a remote configuration source.
/// </summary>
public static class RemoteConfigurationExtensions
{
    /// <summary>
    /// Adds a remote configuration source.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="configure">Configures the source.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/> that can be used to further configure the configuration.</returns>
    public static IConfigurationBuilder AddRemoteConfiguration(this IConfigurationBuilder builder, Action<RemoteConfigurationOptions> configure)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new RemoteConfigurationOptions() { ServiceUri = "" };
        configure(options);

        var remoteBuilder = new RemoteConfigurationBuilder(builder, options);

        return remoteBuilder;
    }

    public static ServiceCollection AddConfigurationServices(this ServiceCollection services)
    {
        services.AddSingleton<IHashProvider, HashProvider>();

        return services;
    }
}
