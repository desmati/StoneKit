namespace Trustee.Host;

using Microsoft.Extensions.DependencyInjection;

using System;

using Trustee.Publishers;

public static class CustomPublisherExtensions
{
    /// <summary>
    /// Adds a custom configuration publisher.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationServiceBuilder"/> to add services to.</param>
    /// <param name="publisher">The custom implementation of <see cref="IPublisher"/>.</param>
    /// <returns>An <see cref="IConfigurationServiceBuilder"/> that can be used to further configure the 
    /// ConfigurationService services.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="builder"/> or <paramref name="publisher"/> is null.
    /// </exception>
    public static IConfigurationServiceBuilder AddPublisher(this IConfigurationServiceBuilder builder, IPublisher publisher)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (publisher == null)
        {
            throw new ArgumentNullException(nameof(publisher));
        }

        // Register the custom configuration publisher service
        builder.Services.AddSingleton(publisher);

        return builder;
    }
}
