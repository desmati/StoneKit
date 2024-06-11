namespace Trustee.Publishers;

using Microsoft.Extensions.DependencyInjection;

using NATS.Client;

using System;

using Trustee.Host;


/// <summary>
/// Provides extension methods to register NATS as a configuration publisher.
/// </summary>
public static class NatPublisherExtensions
{
    /// <summary>
    /// Adds NATS as the configuration publisher.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationServiceBuilder"/> to add services to.</param>
    /// <param name="configure">Configure options for the NATS connection.</param>
    /// <returns>An <see cref="IConfigurationServiceBuilder"/> that can be used to further configure the 
    /// ConfigurationService services.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="builder"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static IConfigurationServiceBuilder AddNatsPublisher(this IConfigurationServiceBuilder builder, Action<Options> configure)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        // Create NATS connection options
        var options = ConnectionFactory.GetDefaultOptions();
        configure(options);

        // Register NATS connection options and publisher service
        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IPublisher, NatsPublisher>();

        return builder;
    }
}
