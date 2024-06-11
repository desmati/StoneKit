namespace Trustee.Host;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents a builder for configuring services in the configuration service.
/// </summary>
public interface IConfigurationServiceBuilder
{
    /// <summary>
    /// Gets the service collection to which configuration services are added.
    /// </summary>
    IServiceCollection Services { get; }
}

