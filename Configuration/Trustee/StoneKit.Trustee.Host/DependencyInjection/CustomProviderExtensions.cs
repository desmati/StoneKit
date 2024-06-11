namespace Trustee.Host;

using Microsoft.Extensions.DependencyInjection;

using System;

using Trustee.Providers;


public static class CustomProviderExtensions
{
    /// <summary>
    /// Adds a custom storage provider backend to the configuration service.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationServiceBuilder"/> to add services to.</param>
    /// <param name="provider">The custom implementation of <see cref="IProvider"/>.</param>
    /// <returns>An <see cref="IConfigurationServiceBuilder"/> that can be used to further configure the ConfigurationService services.</returns>
    public static IConfigurationServiceBuilder AddProvider(this IConfigurationServiceBuilder builder, IProvider provider)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder), "The configuration service builder cannot be null.");
        }

        if (provider == null)
        {
            throw new ArgumentNullException(nameof(provider), "The provider implementation cannot be null.");
        }

        // Register the custom provider as a singleton service
        builder.Services.AddSingleton(provider);

        return builder;
    }

}
