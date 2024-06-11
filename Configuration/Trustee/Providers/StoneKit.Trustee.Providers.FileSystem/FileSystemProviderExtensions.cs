namespace Trustee.Providers;

using Microsoft.Extensions.DependencyInjection;

using Trustee.Host;


/// <summary>
/// Provides extension methods to add the file system provider to the configuration service.
/// </summary>
public static class FileSystemProviderExtensions
{
    /// <summary>
    /// Adds the file system as the storage provider backend.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationServiceBuilder"/> to add services to.</param>
    /// <param name="configure">The action to configure file system provider options.</param>
    /// <returns>An <see cref="IConfigurationServiceBuilder"/> that can be used to further configure the ConfigurationService services.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> or <paramref name="configure"/> is null.</exception>
    public static IConfigurationServiceBuilder AddFileSystemProvider(this IConfigurationServiceBuilder builder, Action<FileSystemProviderOptions> configure)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new FileSystemProviderOptions() { Path="" };
        configure(options);

        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IProvider, FileSystemProvider>();

        return builder;
    }
}
