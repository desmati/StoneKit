namespace System.Data;

/// <summary>
/// Represents the options to configure the data store.
/// </summary>
public class DataStoreOptions
{
    /// <summary>
    /// Gets or sets the directory path where data will be stored.
    /// </summary>
    public string? DirectoryPath { get; set; }

    /// <summary>
    /// Gets or sets the time span after which files should be considered for cleanup. 
    /// If not set, no cleanup will occure.
    /// </summary>
    public TimeSpan? FileLifetime { get; set; }

    /// <summary>
    /// Gets or sets the interval at which the cleanup task will run.
    /// If FileLifetime is set, specify how offen start the cleanup process.
    /// Defaul is 60 minutes.
    /// </summary>
    public TimeSpan? CleanupInterval { get; set; }
}
