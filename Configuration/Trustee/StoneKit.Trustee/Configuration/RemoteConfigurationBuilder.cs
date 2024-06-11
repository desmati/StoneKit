namespace Trustee.Client;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents a builder for remote configuration.
/// </summary>
internal class RemoteConfigurationBuilder : IConfigurationBuilder
{
    private readonly IConfigurationBuilder _configurationBuilder;
    private readonly RemoteConfigurationOptions _remoteConfigurationOptions;

    /// <inheritdoc />
    public IDictionary<string, object> Properties => _configurationBuilder.Properties;

    /// <inheritdoc />
    public IList<IConfigurationSource> Sources => _configurationBuilder.Sources;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteConfigurationBuilder"/> class.
    /// </summary>
    /// <param name="configurationBuilder">The underlying configuration builder.</param>
    /// <param name="remoteConfigurationOptions">The options for remote configuration.</param>
    public RemoteConfigurationBuilder(IConfigurationBuilder configurationBuilder, RemoteConfigurationOptions remoteConfigurationOptions)
    {
        _configurationBuilder = configurationBuilder ?? throw new ArgumentNullException(nameof(configurationBuilder));
        _remoteConfigurationOptions = remoteConfigurationOptions ?? throw new ArgumentNullException(nameof(remoteConfigurationOptions));
    }

    /// <inheritdoc />
    public IConfigurationBuilder Add(IConfigurationSource source)
    {
        return _configurationBuilder.Add(source);
    }

    /// <inheritdoc />
    public IConfigurationRoot Build()
    {
        foreach (var configuration in _remoteConfigurationOptions.Configurations)
        {
            var source = new RemoteConfigurationSource
            {
                ConfigurationServiceUri = _remoteConfigurationOptions.ServiceUri,
                HttpMessageHandler = _remoteConfigurationOptions.HttpMessageHandler,
                RequestTimeout = _remoteConfigurationOptions.RequestTimeout,

                ConfigurationName = configuration.ConfigurationName,
                Optional = configuration.Optional,
                ReloadOnChange = configuration.ReloadOnChange,
                Parser = configuration.Parser,

                CreateSubscriber = _remoteConfigurationOptions.CreateSubscriber
            };

            Add(source);
        }

        return _configurationBuilder.Build();
    }
}
