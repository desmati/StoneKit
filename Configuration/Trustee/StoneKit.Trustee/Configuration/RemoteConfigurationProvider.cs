namespace Trustee.Client;

using Microsoft.Extensions.Configuration;

using Serilog;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Provides configuration from a remote source.
/// </summary>
internal class RemoteConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly RemoteConfigurationSource _source;
    private readonly Lazy<HttpClient> _httpClient;
    private readonly IConfigurationParser _parser;
    private bool _disposed;

    private string? Hash { get; set; }

    private HttpClient HttpClient => _httpClient.Value;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteConfigurationProvider"/> class.
    /// </summary>
    /// <param name="source">The configuration source.</param>
    public RemoteConfigurationProvider(RemoteConfigurationSource source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));

        if (string.IsNullOrWhiteSpace(source.ConfigurationName))
        {
            throw new RemoteConfigurationException(nameof(source.ConfigurationName));
        }

        if (string.IsNullOrWhiteSpace(source.ConfigurationServiceUri))
        {
            throw new RemoteConfigurationException(nameof(source.ConfigurationServiceUri));
        }

        Log.Information("Initializing remote configuration source for configuration '{ConfigurationName}'", source.ConfigurationName);

        _httpClient = new Lazy<HttpClient>(CreateHttpClient);

        _parser = source.Parser ?? GetDefaultParser();

        Log.Information("Using parser {Name}", _parser.GetType().Name);

        SubscribeToConfigurationChanges();
    }

    /// <summary>
    /// Loads the configuration data from the remote source.
    /// </summary>
    public override void Load() => LoadAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Disposes the resources used by the <see cref="RemoteConfigurationProvider"/>.
    /// </summary>
    /// <param name="disposing">True if disposing; otherwise, false.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing && _httpClient?.IsValueCreated == true)
        {
            _httpClient.Value.Dispose();
        }

        _disposed = true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private HttpClient CreateHttpClient()
    {
        var handler = _source.HttpMessageHandler ?? new HttpClientHandler();
        var client = new HttpClient(handler, true)
        {
            BaseAddress = new Uri(_source.ConfigurationServiceUri),
            Timeout = _source.RequestTimeout
        };

        return client;
    }

    private async Task LoadAsync()
    {
        Data = await RequestConfigurationAsync().ConfigureAwait(false);
    }

    private async Task<IDictionary<string, string>> RequestConfigurationAsync()
    {
        var encodedConfigurationName = WebUtility.UrlEncode(_source.ConfigurationName);

        Log.Information("Requesting remote configuration {ConfigurationName} from {BaseAddress}",
            _source.ConfigurationName, HttpClient.BaseAddress);

        try
        {
            using (var response = await HttpClient.GetAsync(encodedConfigurationName).ConfigureAwait(false))
            {
                Log.Information("Received response status code {StatusCode} from endpoint for configuration '{ConfigurationName}'",
                    response.StatusCode, _source.ConfigurationName);

                if (response.IsSuccessStatusCode)
                {
                    using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        Log.Information("Parsing remote configuration response stream " +
                                                "({Length:N0} bytes) for configuration '{ConfigurationName}'",
                            stream.Length, _source.ConfigurationName);

                        Hash = ComputeHash(stream);
                        Log.Information("Computed hash for Configuration '{ConfigurationName}' is {Hash}",
                            _source.ConfigurationName, Hash);

                        stream.Position = 0;
                        var data = _parser.Parse(stream);

                        Log.Information("Configuration updated for '{ConfigurationName}'", _source.ConfigurationName);

                        return data;
                    }
                }

                if (!_source.Optional)
                {
                    throw new HttpRequestException($"Error calling remote configuration endpoint: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }
        catch (Exception)
        {
            if (!_source.Optional)
            {
                throw;
            }
        }

        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    private string ComputeHash(Stream stream)
    {
        using (var hash = SHA1.Create())
        {
            var hashBytes = hash.ComputeHash(stream);

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }

    private IConfigurationParser GetDefaultParser()
    {
        var extension = Path.GetExtension(_source.ConfigurationName).ToLower();

        Log.Information("A file parser was not specified. Attempting to resolve parser from file extension '{Extension}'", extension);

        switch (extension)
        {
            case ".ini":
                return new IniConfigurationFileParser();
            case ".xml":
                return new XmlConfigurationFileParser();
            case ".yaml":
                return new YamlConfigurationFileParser();
            default:
                return new JsonConfigurationFileParser();
        }
    }

    private void SubscribeToConfigurationChanges()
    {
        if (_source.ReloadOnChange)
        {
            if (_source.CreateSubscriber == null)
            {
                Log.Warning("ReloadOnChange is enabled but a subscriber has not been configured");
                return;
            }

            var subscriber = _source.CreateSubscriber();

            Log.Information("Initializing remote configuration {Name} subscriber for configuration '{ConfigurationName}'",
                subscriber.Name, _source.ConfigurationName);

            subscriber.Initialize();

            subscriber.Subscribe(_source.ConfigurationName, message =>
            {
                Log.Information("Received remote configuration change subscription for configuration '{ConfigurationName}' with hash {Message}",
                    _source.ConfigurationName, message);

                Log.Information("Current hash is {Hash}", Hash);

                if (message != null && message.Equals(Hash, StringComparison.OrdinalIgnoreCase))
                {
                    Log.Information("Configuration '{ConfigurationName}' current hash {Hash} matches new hash",
                        _source.ConfigurationName, Hash);

                    Log.Information("Configuration will not be updated");

                    return;
                }

                Load();
                OnReload();
            });
        }
    }
}
