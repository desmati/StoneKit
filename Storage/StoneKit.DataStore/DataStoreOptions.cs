namespace System.Data;

/// <summary>
/// Represents the options to configure the data store.
/// </summary>
public class DataStoreOptions
{
    /// <summary>
    /// Gets or sets the directory path where data will be stored. Set it as file path if only a single file will be stored.
    /// </summary>
    public string? DirectoryOrFilePath { get; set; }

    /// <summary>
    /// Gets or sets an encryption key used for encrypting/decrypting file contents using AES. No encryption will be used if left blank or null.
    /// </summary>
    public string? EncryptionKey { get; set; }

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
