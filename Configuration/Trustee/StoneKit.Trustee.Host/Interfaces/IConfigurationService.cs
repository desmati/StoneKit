namespace Trustee.Host;

/// <summary>
/// Defines the contract for a service that manages configuration settings.
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Initializes the configuration service.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    Task Initialize(CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles the event when configuration changes are detected.
    /// </summary>
    /// <param name="paths">A collection of paths representing the changed configuration items.</param>
    /// <returns>A task that represents the asynchronous operation of handling configuration changes.</returns>
    Task OnChange(IEnumerable<string> paths);

    /// <summary>
    /// Publishes the changes to the configuration.
    /// </summary>
    /// <param name="paths">A collection of paths representing the configuration items to be published.</param>
    /// <returns>A task that represents the asynchronous operation of publishing configuration changes.</returns>
    Task PublishChanges(IEnumerable<string> paths);
}
