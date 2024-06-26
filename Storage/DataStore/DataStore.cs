using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace System.Data;

/// <summary>
/// A simple data store for storing and retrieving objects by ID.
/// </summary>
/// <typeparam name="T">The type of objects to store.</typeparam>
public class DataStore<T> : IDisposable where T : class
{
    const int DEFAULT_CLEANUP_INTERVAL_IN_MINUTES = 60;

    private readonly string _directoryPath;
    private readonly ConcurrentDictionary<string, string> _filePaths;
    private readonly Timer? _cleanupTimer;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataStore{T}"/> class.
    /// </summary>
    /// <param name="directoryPath">The directory path where data will be stored.</param>
    /// <param name="fileLifetime">The time span after which files should be considered for cleanup.</param>
    /// <param name="cleanupInterval">The interval at which the cleanup task will run.</param>
    public DataStore(string? directoryPath, TimeSpan? fileLifetime = null, TimeSpan? cleanupInterval = null)
    {
        _directoryPath = Path.Combine(directoryPath ?? $"./DataStore", typeof(T).Name);
        _filePaths = new ConcurrentDictionary<string, string>();

        // Ensure the directory exists
        if (!Directory.Exists(_directoryPath))
        {
            Directory.CreateDirectory(_directoryPath);
        }

        _cleanupTimer = SetupCleanup(fileLifetime, cleanupInterval);
    }

    /// <summary>
    /// Sets up the cleanup task if a valid file lifetime is provided.
    /// </summary>
    /// <param name="fileLifetime">The time span after which files should be considered for cleanup.</param>
    /// <param name="cleanupInterval">The interval at which the cleanup task will run.</param>
    /// <returns>A Timer instance if cleanup is configured; otherwise, null.</returns>
    private Timer? SetupCleanup(TimeSpan? fileLifetime, TimeSpan? cleanupInterval)
    {
        if (fileLifetime == null || fileLifetime.Value.TotalMilliseconds <= 0)
        {
            return null;
        }

        // Setup and start the cleanup timer
        return new Timer((state) =>
        {
            // Performs the cleanup task to delete files older than the specified file lifetime.
            var files = Directory.GetFiles(_directoryPath);
            foreach (var file in files)
            {
                if (File.GetCreationTime(file) < DateTime.Now - fileLifetime)
                {
                    File.Delete(file);
                    var id = Path.GetFileNameWithoutExtension(file);
                    _filePaths.TryRemove(id, out _); // Remove from the cache
                }
            }
        }, null, TimeSpan.Zero, cleanupInterval ?? TimeSpan.FromMinutes(DEFAULT_CLEANUP_INTERVAL_IN_MINUTES));
    }

    /// <summary>
    /// Saves an object to the data store asynchronously.
    /// </summary>
    /// <param name="data">The object to save.</param>
    /// <param name="id">Optional. The ID to associate with the object. If not provided, an ID will be computed from the data.</param>
    /// <returns>A task representing the asynchronous save operation. The task result contains the ID associated with the saved object.</returns>
    public async Task<string> SaveAsync(T data, string? id = null)
    {
        var json = JsonSerializer.Serialize(data);

        if (string.IsNullOrEmpty(id))
        {
            id = ComputeId(json);
        }

        var filePath = GetFilePath(id);
        await File.WriteAllTextAsync(filePath, json);
        _filePaths[id] = filePath; // Cache the file path

        return id;
    }

    /// <summary>
    /// Loads an object from the data store asynchronously.
    /// </summary>
    /// <param name="id">The ID associated with the object.</param>
    /// <returns>A task representing the asynchronous load operation. The task result contains the object.</returns>
    public async Task<T?> LoadAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return default;
        }

        if (_filePaths.TryGetValue(id, out var filePath) && File.Exists(filePath))
        {
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T?>(json);
        }

        return default; // Return null if the object is not found
    }

    /// <summary>
    /// Gets the file path for the given ID.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <returns>The file path.</returns>
    private string GetFilePath(string id) => Path.Combine(_directoryPath, $"{id}.json");

    /// <summary>
    /// Computes an ID for the given input string using MD5 hashing.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The computed ID.</returns>
    private static string ComputeId(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Disposes the resources used by the DataStore.
    /// </summary>
    /// <param name="disposing">A value indicating whether the method is called from Dispose.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _cleanupTimer?.Dispose();
                _filePaths?.Clear();
            }

            disposedValue = true;
        }
    }

    /// <summary>
    /// Disposes the DataStore and suppresses finalization.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
