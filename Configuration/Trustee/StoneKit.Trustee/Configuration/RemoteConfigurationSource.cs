namespace Trustee.Client;

using Microsoft.Extensions.Configuration;

using System;

using Trustee.Subscribers;

/// <summary>
/// Represents a remote file as an <see cref="IConfigurationSource"/>.
/// </summary>
internal class RemoteConfigurationSource : IConfigurationSource
{
    /// <summary>
    /// Gets or sets the configuration service endpoint.
    /// </summary>
    public required string ConfigurationServiceUri { get; set; }

    /// <summary>
    /// Gets or sets the name or path of the configuration file relative to the configuration provider path.
    /// </summary>
    public required string ConfigurationName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether loading the file is optional. Defaults to false.
    /// </summary>
    public bool Optional { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the source will be loaded if the underlying file changes. Defaults to false.
    /// </summary>
    public bool ReloadOnChange { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="System.Net.Http.HttpMessageHandler"/> for the <see cref="HttpClient"/>.
    /// </summary>
    public HttpMessageHandler? HttpMessageHandler { get; set; }

    /// <summary>
    /// Gets or sets the timeout for the <see cref="HttpClient"/> request to the configuration server.
    /// </summary>
    public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// Gets or sets the type of <see cref="IConfigurationParser"/> used to parse the remote configuration file.
    /// </summary>
    public IConfigurationParser? Parser { get; set; }

    /// <summary>
    /// Gets or sets the delegate to create the type of <see cref="ISubscriber"/> used to subscribe to published configuration messages.
    /// </summary>
    public Func<ISubscriber>? CreateSubscriber { get; set; }

    /// <summary>
    /// Builds the <see cref="IConfigurationProvider"/> for this <see cref="RemoteConfigurationSource"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to build upon.</param>
    /// <returns>An <see cref="IConfigurationProvider"/> configured with this <see cref="RemoteConfigurationSource"/>.</returns>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new RemoteConfigurationProvider(this);
    }
}
