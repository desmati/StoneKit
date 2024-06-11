namespace Trustee.Providers;

using Serilog;

using System.Net;
using System.Security.Cryptography;

using Trustee.Host;

/// <summary>
/// Represents a configuration provider that retrieves configuration data from the file system.
/// </summary>
public class FileSystemProvider : IProvider
{
    private readonly FileSystemProviderOptions _providerOptions;
    private readonly IHashProvider _hashProvider;
    private FileSystemWatcher? _fileSystemWatcher = null;
    private Func<IEnumerable<string>, Task>? _onChange = null;

    /// <summary>
    /// Gets the name of the configuration provider.
    /// </summary>
    public string Name => "File System";

    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemProvider"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="providerOptions">The options for the file system provider.</param>
    public FileSystemProvider(FileSystemProviderOptions providerOptions, IHashProvider hashProvider)
    {
        _providerOptions = providerOptions ?? throw new ArgumentNullException(nameof(providerOptions));

        if (string.IsNullOrWhiteSpace(_providerOptions.Path))
        {
            throw new ProviderOptionNullException(nameof(_providerOptions.Path));
        }

        _hashProvider = hashProvider;
    }

    /// <summary>
    /// Begins watching for changes in the configuration.
    /// </summary>
    /// <param name="onChange">A callback function to be invoked when configuration changes occur.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous watching operation.</returns>
    public Task Watch(Func<IEnumerable<string>, Task> onChange, CancellationToken cancellationToken = default)
    {
        _onChange = onChange ?? throw new ArgumentNullException(nameof(onChange));

        if (_fileSystemWatcher !=  null)
        {
            // Enable raising events to start watching for file system changes
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Initializes the file system provider.
    /// </summary>
    public void Initialize()
    {
        // Logging initialization details
        Log.Information("Initializing {Name} provider with options {@Options}", Name, new
        {
            _providerOptions.Path,
            _providerOptions.SearchPattern,
            _providerOptions.IncludeSubdirectories
        });

        // Set up network credentials if provided
        if (_providerOptions.Username != null && _providerOptions.Password != null)
        {
            var credentials = new NetworkCredential(_providerOptions.Username, _providerOptions.Password, _providerOptions.Domain);
            var uri = new Uri(_providerOptions.Path);

            // Add credentials to CredentialCache for the specified URI
            _ = new CredentialCache
                {
                    { new Uri($"{uri.Scheme}://{uri.Host}"), "Basic", credentials }
                };
        }

        // Initialize file system watcher
        _fileSystemWatcher = new FileSystemWatcher
        {
            Path = _providerOptions.Path,
            Filter = _providerOptions.SearchPattern ?? "*.*",
            IncludeSubdirectories = _providerOptions.IncludeSubdirectories,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
        };

        // Subscribe to events
        _fileSystemWatcher.Created += FileSystemWatcher_Changed;
        _fileSystemWatcher.Changed += FileSystemWatcher_Changed;
    }

    /// <summary>
    /// Retrieves the configuration data for the specified name.
    /// </summary>
    /// <param name="name">The name of the configuration to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation of retrieving configuration data.</returns>
    public async Task<byte[]> GetConfiguration(string name)
    {
        string path = Path.Combine(_providerOptions.Path, name);

        if (!File.Exists(path))
        {
            Log.Information("File does not exist at {Path}", path);
            return [];
        }

        return await File.ReadAllBytesAsync(path);
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
    public Task<IEnumerable<string>> ListPaths()
    {
        Log.Information("Listing files at {Path}", _providerOptions.Path);

        var searchOption = _providerOptions.IncludeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var files = Directory.EnumerateFiles(_providerOptions.Path, _providerOptions.SearchPattern ?? "*", searchOption);
        var relativePaths = new List<string>();

        foreach (var file in files)
        {
            relativePaths.Add(GetRelativePath(file));
        }

        Log.Information("{Count} files found", relativePaths.Count);

        return Task.FromResult<IEnumerable<string>>(relativePaths);
    }

    private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        Log.Information("Detected file change at {FullPath}", e.FullPath);

        var filename = GetRelativePath(e.FullPath);

        if (_onChange != null)
        {
            _onChange([filename]);
        }
    }

    private string GetRelativePath(string fullPath)
    {
        return Path.GetRelativePath(_providerOptions.Path, fullPath);
    }
}
