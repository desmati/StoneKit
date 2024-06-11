namespace Trustee.Host;

using Microsoft.Extensions.DependencyInjection;

using System;

/// <summary>
/// Represents a builder for configuring services in the ConfigurationService.
/// </summary>
public class ConfigurationServiceBuilder : IConfigurationServiceBuilder
{
    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> where services are configured.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationServiceBuilder"/> class.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    public ConfigurationServiceBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }
}
