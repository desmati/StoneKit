namespace Trustee.Providers;

using Microsoft.Extensions.Logging;

using Serilog;

using System.Security.Cryptography;
using System.Text.Json;

using Trustee.Host;

using VaultSharp;

/// <summary>
/// Represents a configuration provider that retrieves secrets from HashiCorp Vault. 
/// https://www.hashicorp.com/products/vault
/// </summary>
public class VaultProvider : IProvider
{
    private readonly IHashProvider _hashProvider;
    private readonly VaultProviderOptions _providerOptions;
    private readonly IDictionary<string, int> _secretVersions = new Dictionary<string, int>();

    private IVaultClient? _vaultClient;

    /// <summary>
    /// Gets the name of the Vault provider.
    /// </summary>
    public string Name => "Vault";

    /// <summary>
    /// Initializes a new instance of the <see cref="VaultProvider"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="providerOptions">The options for the Vault provider.</param>
    /// <exception cref="ProviderOptionNullException">Thrown when essential provider options are not provided.</exception>
    public VaultProvider(ILogger<VaultProvider> logger, VaultProviderOptions providerOptions, IHashProvider hashProvider)
    {
        _providerOptions = providerOptions ?? throw new ArgumentNullException(nameof(providerOptions));

        if (string.IsNullOrWhiteSpace(_providerOptions.ServerUri))
        {
            throw new ProviderOptionNullException(nameof(_providerOptions.ServerUri));
        }

        if (string.IsNullOrWhiteSpace(_providerOptions.Path))
        {
            throw new ProviderOptionNullException(nameof(_providerOptions.Path));
        }

        if (_providerOptions.AuthMethodInfo == null)
        {
            throw new ProviderOptionNullException(nameof(_providerOptions.AuthMethodInfo));
        }

        _hashProvider = hashProvider;
    }

    /// <summary>
    /// Watches for changes in Vault secrets and triggers a callback when changes are detected.
    /// </summary>
    /// <param name="onChange">The callback function to be invoked when changes occur.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous watching operation.</returns>
    public async Task Watch(Func<IEnumerable<string>, Task> onChange, CancellationToken cancellationToken = default)
    {
        // Continuously monitor for changes until cancellation is requested.
        while (!cancellationToken.IsCancellationRequested && _vaultClient != null)
        {
            try
            {
                var changes = new List<string>();

                // List all secret paths in Vault.
                var paths = await ListPaths();

                // Check for changes in each secret.
                foreach (var path in paths)
                {
                    var metadata = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(path, _providerOptions.Path);

                    // Compare current secret version with stored version.
                    _secretVersions.TryGetValue(path, out int version);
                    if (version != metadata.Data.CurrentVersion)
                    {
                        changes.Add(path);
                        _secretVersions[path] = metadata.Data.CurrentVersion;
                    }
                }

                // Trigger callback if changes are detected.
                if (changes.Count > 0)
                {
                    await onChange(changes);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred while attempting to poll for changes");
            }

            // Calculate next polling period.
            var delayDate = DateTime.UtcNow.Add(_providerOptions.PollingInterval);
            Log.Information("Next polling period will begin in {PollingInterval:c} at {DelayDate}",
                _providerOptions.PollingInterval, delayDate);

            // Delay for the specified polling interval.
            await Task.Delay(_providerOptions.PollingInterval, cancellationToken);
        }
    }

    /// <summary>
    /// Initializes the Vault provider by setting up the Vault client.
    /// </summary>
    public void Initialize()
    {
        Log.Information("Initializing {Name} provider with options {@Options}", Name, new
        {
            _providerOptions.ServerUri,
            _providerOptions.Path
        });

        // Configure Vault client settings.
        var vaultClientSettings = new VaultClientSettings(_providerOptions.ServerUri, _providerOptions.AuthMethodInfo);
        _vaultClient = new VaultClient(vaultClientSettings);
    }

    /// <summary>
    /// Retrieves the configuration data (secret) from Vault for the specified name.
    /// </summary>
    /// <param name="name">The name of the secret to retrieve.</param>
    /// <returns>A task representing the asynchronous operation of retrieving configuration data.</returns>
    public async Task<byte[]> GetConfiguration(string name)
    {
        if (_vaultClient == null)
        {
            return [];
        }

        // Read the secret data from Vault.
        var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(name, null, _providerOptions.Path);

        // Check if secret exists.
        if (secret == null)
        {
            Log.Information("Secret does not exist at {Name}", name);
            return [];
        }

        // Serialize secret data to JSON format.
        await using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, secret.Data.Data);
        return stream.ToArray();
    }

    /// <summary>
    /// Computes the hash value of the configuration data retrieved from Vault for the specified name.
    /// </summary>
    /// <param name="name">The name of the secret to compute the hash value for.</param>
    /// <returns>A task representing the asynchronous operation of computing the hash value.</returns>
    public async Task<string> GetHash(string name)
    {
        var bytes = await GetConfiguration(name);
        return _hashProvider.ComputeHashString(bytes);
    }

    /// <summary>
    /// Lists all secret paths available in the specified Vault path.
    /// </summary>
    /// <returns>A task representing the asynchronous operation of listing secret paths.</returns>
    public async Task<IEnumerable<string>> ListPaths()
    {
        if (_vaultClient == null)
        {
            return [];
        }

        Log.Information("Listing paths at {Path}", _providerOptions.Path);

        // Read all secret paths from Vault.
        var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretPathsAsync(null, _providerOptions.Path);
        var paths = secret.Data.Keys.ToList();

        Log.Information("{Count} paths found", paths.Count);
        return paths;
    }
}
