namespace Trustee.Providers;

using Microsoft.Extensions.DependencyInjection;

using System;

using Trustee.Host;

/// <summary>
/// Provides extension methods to add the Vault provider to the configuration service.
/// </summary>
public static class VaultProviderExtensions
{
    /// <summary>
    /// Adds Vault as the storage provider backend.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationServiceBuilder"/> to add services to.</param>
    /// <param name="configure">The action to configure Vault provider options.</param>
    /// <returns>An <see cref="IConfigurationServiceBuilder"/> that can be used to further configure the ConfigurationService services.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> or <paramref name="configure"/> is null.</exception>
    public static IConfigurationServiceBuilder AddVaultProvider(this IConfigurationServiceBuilder builder, Action<VaultProviderOptions> configure)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new VaultProviderOptions();
        configure(options);

        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IProvider, VaultProvider>();

        return builder;
    }
}
