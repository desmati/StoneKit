namespace Trustee.Publishers;

using Microsoft.Extensions.DependencyInjection;

using System;

using Trustee.Host;


/// <summary>
/// Provides extension methods to register RabbitMq as a configuration publisher.
/// </summary>
public static class RabbitMqPublisherExtensions
{
    /// <summary>
    /// Adds RabbitMQ as the configuration publisher.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationServiceBuilder"/> to add services to.</param>
    /// <param name="configure">A delegate to configure options for the RabbitMQ publisher.</param>
    /// <returns>
    /// An <see cref="IConfigurationServiceBuilder"/> that can be used to further configure the 
    /// ConfigurationService services.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="builder"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static IConfigurationServiceBuilder AddRabbitMqPublisher(this IConfigurationServiceBuilder builder, Action<RabbitMqOptions> configure)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        // Create RabbitMqOptions instance and configure it
        var options = new RabbitMqOptions();
        configure(options);

        // Register RabbitMqOptions and RabbitMqPublisher as singleton services
        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IPublisher, RabbitMqPublisher>();

        return builder;
    }

}
