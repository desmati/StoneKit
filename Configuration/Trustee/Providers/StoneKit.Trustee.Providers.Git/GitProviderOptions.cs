namespace Trustee.Providers;

/// <summary>
/// Options for <see cref="GitProvider"/>.
/// </summary>
public class GitProviderOptions
{
    /// <summary>
    /// Gets or sets the URI for the remote repository.
    /// </summary>
    public required string RepositoryUrl { get; set; }

    /// <summary>
    /// Gets or sets the username for authentication.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password for authentication.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the name of the branch to checkout. When unspecified, the remote's default branch will be used instead.
    /// </summary>
    public string? Branch { get; set; }

    /// <summary>
    /// Gets or sets the local path to clone into.
    /// </summary>
    public string? LocalPath { get; set; }

    /// <summary>
    /// Gets or sets the search string to use as a filter against the names of files. Defaults to all files ('*').
    /// </summary>
    public string? SearchPattern { get; set; }

    /// <summary>
    /// Gets or sets the interval to check for remote changes. Defaults to 60 seconds.
    /// </summary>
    public TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(60);
}

