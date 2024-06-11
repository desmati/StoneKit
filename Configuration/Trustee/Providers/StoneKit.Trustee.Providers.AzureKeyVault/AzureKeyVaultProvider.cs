using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using Microsoft.Extensions.Logging;

using Serilog;

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;

namespace Trustee.Providers
{
    /// <summary>
    /// Represents a configuration provider that retrieves secrets from Azure Key Vault.
    /// </summary>
    public class AzureKeyVaultProvider : IProvider
    {
        private readonly IHashProvider _hashProvider;
        private readonly AzureKeyVaultProviderOptions _providerOptions;
        private readonly IDictionary<string, string> _secretVersions = new ConcurrentDictionary<string, string>();
        private SecretClient? _secretClient;

        /// <summary>
        /// Gets the name of the configuration provider.
        /// </summary>
        public string Name => "AzureKeyVault";

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureKeyVaultProvider"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="providerOptions">The options for the Azure Key Vault provider.</param>
        /// <param name="hashProvider">The hash provider instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when essential provider options are not provided.</exception>
        public AzureKeyVaultProvider(ILogger<AzureKeyVaultProvider> logger, AzureKeyVaultProviderOptions providerOptions, IHashProvider hashProvider)
        {
            _providerOptions = providerOptions ?? throw new ArgumentNullException(nameof(providerOptions));
            _hashProvider = hashProvider ?? throw new ArgumentNullException(nameof(hashProvider));

            if (string.IsNullOrWhiteSpace(_providerOptions.VaultUri))
            {
                throw new ArgumentException("VaultUri is required.", nameof(_providerOptions.VaultUri));
            }
        }

        /// <summary>
        /// Initializes the configuration provider by setting up the Azure Key Vault client.
        /// </summary>
        public void Initialize()
        {
            Log.Information("Initializing {Name} provider with options {@Options}", Name, new
            {
                _providerOptions.VaultUri
            });

            _secretClient = new SecretClient(new Uri(_providerOptions.VaultUri), new DefaultAzureCredential());
        }

        /// <summary>
        /// Begins watching for changes in the configuration.
        /// </summary>
        /// <param name="onChange">A callback function to be invoked when configuration changes occur.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous watching operation.</returns>
        public async Task Watch(Func<IEnumerable<string>, Task> onChange, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested && _secretClient != null)
            {
                try
                {
                    var changes = new List<string>();
                    var secrets = _secretClient.GetPropertiesOfSecretsAsync().ToBlockingEnumerable(cancellationToken);

                    foreach (var secret in secrets)
                    {

                        if (_secretVersions.TryGetValue(secret.Name, out var currentVersion) &&
                            currentVersion != secret.Version)
                        {
                            changes.Add(secret.Name);
                            _secretVersions[secret.Name] = secret.Version;
                        }
                    }

                    if (changes.Count > 0)
                    {
                        await onChange(changes);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An unhandled exception occurred while attempting to poll for changes.");
                }

                var delayDate = DateTime.UtcNow.Add(_providerOptions.PollingInterval);
                Log.Information("Next polling period will begin in {PollingInterval:c} at {DelayDate}", _providerOptions.PollingInterval, delayDate);

                await Task.Delay(_providerOptions.PollingInterval, cancellationToken);
            }
        }

        /// <summary>
        /// Retrieves the configuration data for the specified name.
        /// </summary>
        /// <param name="name">The name of the configuration to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation of retrieving configuration data.</returns>
        public async Task<byte[]> GetConfiguration(string name)
        {
            if (_secretClient == null)
            {
                return Array.Empty<byte>();
            }

            try
            {
                var secret = await _secretClient.GetSecretAsync(name);
                await using var stream = new MemoryStream();
                await JsonSerializer.SerializeAsync(stream, secret.Value.Value);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve secret {SecretName}.", name);
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// Retrieves the hash value for the configuration data with the specified name.
        /// </summary>
        /// <param name="name">The name of the configuration to retrieve the hash value for.</param>
        /// <returns>A task that represents the asynchronous operation of retrieving the hash value.</returns>
        public async Task<string> GetHash(string name)
        {
            var bytes = await GetConfiguration(name);
            return _hashProvider.ComputeHashString(bytes);
        }

        /// <summary>
        /// Lists all paths of the configurations available in the provider.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation of listing configuration paths.</returns>
        public async Task<IEnumerable<string>> ListPaths()
        {
            if (_secretClient == null)
            {
                return Enumerable.Empty<string>();
            }

            Log.Information("Listing paths at {VaultUri}", _providerOptions.VaultUri);

            var secrets = _secretClient.GetPropertiesOfSecretsAsync().ToBlockingEnumerable();
            var paths = secrets.Select(s => s.Name).ToList();

            Log.Information("{Count} paths found", paths.Count);
            return paths;
        }
    }

}
