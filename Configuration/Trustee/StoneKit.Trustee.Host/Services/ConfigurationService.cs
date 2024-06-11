namespace Trustee.Host;

using Serilog;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Trustee.Providers;
using Trustee.Publishers;


/// <summary>
/// Service responsible for initializing, watching for changes, and publishing configuration updates.
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private readonly IProvider _provider;
    private readonly IPublisher? _publisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationService"/> class.
    /// </summary>
    /// <param name="provider">The configuration provider.</param>
    /// <param name="publisher">The configuration publisher (optional).</param>
    public ConfigurationService(IProvider provider, IPublisher? publisher = null)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _publisher = publisher;

        if (_publisher == null)
        {
            Log.Information("A publisher has not been configured");
        }
    }

    /// <inheritdoc/>
    public async Task Initialize(CancellationToken cancellationToken = default)
    {
        Log.Information("Initializing {Name} configuration provider...", _provider.Name);

        _provider.Initialize();

        if (_publisher != null)
        {
            Log.Information("Initializing publisher...");
            _publisher.Initialize();
        }

        var paths = await _provider.ListPaths();

        await PublishChanges(paths);

        await _provider.Watch(OnChange, cancellationToken);

        Log.Information("{Name} configuration watching for changes", _provider.Name);
    }

    /// <inheritdoc/>
    public async Task OnChange(IEnumerable<string> paths)
    {
        Log.Information("Changes were detected on the remote {Name} configuration provider", _provider.Name);

        paths = paths ?? throw new ArgumentNullException(nameof(paths));

        if (paths.GetEnumerator().MoveNext())
        {
            await PublishChanges(paths);
        }
    }

    /// <inheritdoc/>
    public async Task PublishChanges(IEnumerable<string> paths)
    {
        if (_publisher == null)
        {
            return;
        }

        Log.Information("Publishing changes...");

        foreach (var path in paths)
        {
            var hash = await _provider.GetHash(path);
            await _publisher.Publish(path, hash);
        }
    }
}
