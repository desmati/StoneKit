namespace Trustee.Providers;

/// <summary>
/// Defines the contract for a configuration provider.
/// </summary>
public interface IProvider
{
    /// <summary>
    /// Gets the name of the configuration provider.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Begins watching for changes in the configuration.
    /// </summary>
    /// <param name="onChange">A callback function to be invoked when configuration changes occur.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous watching operation.</returns>
    Task Watch(Func<IEnumerable<string>, Task> onChange, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initializes the configuration provider.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Retrieves the configuration data for the specified name.
    /// </summary>
    /// <param name="name">The name of the configuration to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation of retrieving configuration data.</returns>
    Task<byte[]> GetConfiguration(string name);

    /// <summary>
    /// Retrieves the hash value for the configuration data with the specified name.
    /// </summary>
    /// <param name="name">The name of the configuration to retrieve the hash value for.</param>
    /// <returns>A task that represents the asynchronous operation of retrieving the hash value.</returns>
    Task<string> GetHash(string name);

    /// <summary>
    /// Lists all paths of the configurations available in the provider.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation of listing configuration paths.</returns>
    Task<IEnumerable<string>> ListPaths();
}
