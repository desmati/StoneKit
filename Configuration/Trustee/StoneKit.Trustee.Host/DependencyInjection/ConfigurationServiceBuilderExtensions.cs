namespace Trustee.Host;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Security.Cryptography;

/// <summary>
/// Provides extension methods to add configuration hosting services to the specified <see cref="IServiceCollection"/>.
/// </summary>
public static class ConfigurationServiceExtensions
{
    /// <summary>
    /// Adds services for configuration hosting to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>An <see cref="IConfigurationServiceBuilder"/> that can be used to further configure the ConfigurationService services.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    public static IConfigurationServiceBuilder AddConfigurationService(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddHostedService<HostedConfigurationService>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<IHashProvider, HashProvider>();

        return new ConfigurationServiceBuilder(services);
    }
}
