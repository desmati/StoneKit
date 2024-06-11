namespace Trustee.Providers;

/// <summary>
/// Options for <see cref="FileSystemProvider"/>.
/// </summary>
public class FileSystemProviderOptions
{
    /// <summary>
    /// Gets or sets the path to the configuration files.
    /// </summary>
    public required string Path { get; set; }

    /// <summary>
    /// Gets or sets the search string to use as a filter against the names of files. Defaults to all files ('*').
    /// </summary>
    public string? SearchPattern { get; set; } = "*";

    /// <summary>
    /// Gets or sets a value indicating whether to include the current directory and all its subdirectories.
    /// </summary>
    public bool IncludeSubdirectories { get; set; }

    /// <summary>
    /// Gets or sets the username for authentication.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password for authentication.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the domain for authentication.
    /// </summary>
    public string? Domain { get; set; }
}
