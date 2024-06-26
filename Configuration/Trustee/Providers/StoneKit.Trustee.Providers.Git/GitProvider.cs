﻿namespace Trustee.Providers;

using LibGit2Sharp;
using LibGit2Sharp.Handlers;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Trustee.Host;

/// <summary>
/// Represents a configuration provider that retrieves configuration data from a Git repository.
/// </summary>
public class GitProvider : IProvider
{
    private readonly ILogger<GitProvider> _logger;
    private readonly GitProviderOptions _providerOptions;
    private readonly IHashProvider _hashProvider;
    private CredentialsHandler? _credentialsHandler;

    /// <summary>
    /// Gets the name of the configuration provider.
    /// </summary>
    public string Name => "Git";

    /// <summary>
    /// Initializes a new instance of the <see cref="GitProvider"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="providerOptions">The options for the Git provider.</param>
    public GitProvider(ILogger<GitProvider> logger, GitProviderOptions providerOptions, IHashProvider hashProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _providerOptions = providerOptions ?? throw new ArgumentNullException(nameof(providerOptions));

        if (string.IsNullOrWhiteSpace(_providerOptions.LocalPath))
        {
            throw new ProviderOptionNullException(nameof(_providerOptions.LocalPath));
        }

        if (string.IsNullOrWhiteSpace(_providerOptions.RepositoryUrl))
        {
            throw new ProviderOptionNullException(nameof(_providerOptions.RepositoryUrl));
        }

        _hashProvider = hashProvider;
    }

    /// <summary>
    /// Watches for changes in the Git repository.
    /// </summary>
    /// <param name="onChange">A callback function to be invoked when configuration changes occur.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous watching operation.</returns>
    public async Task Watch(Func<IEnumerable<string>, Task> onChange, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                List<string> files;

                var task = Task.Run(ListChangedFiles, cancellationToken);
                // The git fetch operation can sometimes hang.  Force to complete after a minute.
                if (task.Wait(TimeSpan.FromSeconds(60)))
                {
                    files = task.Result.ToList();
                }
                else
                {
                    throw new TimeoutException("Attempting to list changed files timed out after 60 seconds.");
                }

                if (files.Count > 0)
                {
                    await onChange(files);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while attempting to poll for changes");
            }

            var delayDate = DateTime.UtcNow.Add(_providerOptions.PollingInterval);

            _logger.LogInformation("Next polling period will begin in {PollingInterval:c} at {DelayDate}",
                _providerOptions.PollingInterval, delayDate);

            await Task.Delay(_providerOptions.PollingInterval, cancellationToken);
        }
    }

    /// <summary>
    /// Initializes the Git provider.
    /// </summary>
    public void Initialize()
    {
        _logger.LogInformation("Initializing {Name} provider with options {@Options}", Name, new
        {
            _providerOptions.RepositoryUrl,
            _providerOptions.LocalPath,
            _providerOptions.Branch,
            _providerOptions.PollingInterval,
            _providerOptions.SearchPattern
        });

        if (Directory.Exists(_providerOptions.LocalPath))
        {
            _logger.LogInformation("A local repository already exists at {LocalPath}", _providerOptions.LocalPath);

            _logger.LogInformation("Deleting directory {LocalPath}", _providerOptions.LocalPath);

            DeleteDirectory(_providerOptions.LocalPath);
        }

        if (!Directory.Exists(_providerOptions.LocalPath) && !string.IsNullOrEmpty(_providerOptions.LocalPath))
        {
            _logger.LogInformation("Creating directory {LocalPath}", _providerOptions.LocalPath);

            Directory.CreateDirectory(_providerOptions.LocalPath);
        }

        if (_providerOptions.Username != null && _providerOptions.Password != null)
        {
            _credentialsHandler = (url, user, cred) => new UsernamePasswordCredentials
            {
                Username = _providerOptions.Username,
                Password = _providerOptions.Password
            };
        }

        var cloneOptions = new CloneOptions
        {
            BranchName = _providerOptions.Branch
        };

        cloneOptions.FetchOptions.CredentialsProvider = _credentialsHandler;

        _logger.LogInformation("Cloning git repository {RepositoryUrl} to {LocalPath}", _providerOptions.RepositoryUrl, _providerOptions.LocalPath);

        var path = Repository.Clone(_providerOptions.RepositoryUrl, _providerOptions.LocalPath, cloneOptions);

        _logger.LogInformation("Repository cloned to {Path}", path);

        using var repo = new Repository(_providerOptions.LocalPath);
        var hash = repo.Head.Tip.Sha.Substring(0, 6);

        _logger.LogInformation("Current HEAD is [{Hash}] '{MessageShort}'", hash, repo.Head.Tip.MessageShort);
    }

    /// <summary>
    /// Retrieves the configuration data for the specified name.
    /// </summary>
    /// <param name="name">The name of the configuration to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation of retrieving configuration data.</returns>
    public async Task<byte[]> GetConfiguration(string name)
    {
        if (string.IsNullOrEmpty(_providerOptions.LocalPath))
        {
            return [];
        }
        string path = Path.Combine(_providerOptions.LocalPath, name);

        if (!File.Exists(path))
        {
            _logger.LogInformation("File does not exist at {Path}", path);
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
        _logger.LogInformation("Listing files at {LocalPath}", _providerOptions.LocalPath);

        IList<string> files = new List<string>();

        if (string.IsNullOrEmpty(_providerOptions.LocalPath))
        {
            return Task.FromResult<IEnumerable<string>>([]);
        }

        using (var repo = new Repository(_providerOptions.LocalPath))
        {
            _logger.LogInformation("Listing files in repository at {LocalPath}", _providerOptions.LocalPath);

            foreach (var entry in repo.Index)
            {
                files.Add(entry.Path.NormalizePathSeparators());
            }
        }

        var localFiles = Directory.EnumerateFiles(_providerOptions.LocalPath, _providerOptions.SearchPattern ?? "*", SearchOption.AllDirectories).ToList();
        localFiles = localFiles.Select(GetRelativePath).ToList();

        files = localFiles.Intersect(files).ToList();

        _logger.LogInformation("{Count} files found", files.Count);

        return Task.FromResult<IEnumerable<string>>(files);
    }

    private async Task<IEnumerable<string>> ListChangedFiles()
    {
        Fetch();

        IList<string> changedFiles = new List<string>();

        using (var repo = new Repository(_providerOptions.LocalPath))
        {
            _logger.LogInformation("Checking for remote changes on {RemoteName}", repo.Head.TrackedBranch.RemoteName);

            foreach (TreeEntryChanges entry in repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, repo.Head.TrackedBranch.Tip.Tree))
            {
                if (entry.Exists)
                {
                    _logger.LogInformation("File {Path} changed", entry.Path);
                    changedFiles.Add(entry.Path.NormalizePathSeparators());
                }
                else
                {
                    _logger.LogInformation("File {Path} no longer exists", entry.Path);
                }
            }
        }

        if (changedFiles.Count == 0)
        {
            _logger.LogInformation("No tree entry changes were detected");

            return changedFiles;
        }

        UpdateLocal();

        var filteredFiles = await ListPaths();
        changedFiles = filteredFiles.Intersect(changedFiles).ToList();

        _logger.LogInformation("{Count} files changed", changedFiles.Count);

        return changedFiles;
    }

    private void UpdateLocal()
    {
        using var repo = new Repository(_providerOptions.LocalPath);
        var options = new PullOptions
        {
            FetchOptions = new FetchOptions
            {
                CredentialsProvider = _credentialsHandler
            }
        };

        var signature = new Signature(new Identity("Configuration Service", "Configuration Service"), DateTimeOffset.Now);

        _logger.LogInformation("Pulling changes to local repository");

        var currentHash = repo.Head.Tip.Sha.Substring(0, 6);

        _logger.LogInformation("Current HEAD is [{CurrentHash}] '{MessageShort}'", currentHash, repo.Head.Tip.MessageShort);

        var result = Commands.Pull(repo, signature, options);

        _logger.LogInformation("Merge completed with status {Status}", result.Status);

        var newHash = result.Commit.Sha.Substring(0, 6);

        _logger.LogInformation("New HEAD is [{NewHash}] '{MessageShort}'", newHash, result.Commit.MessageShort);
    }

    private static void DeleteDirectory(string path)
    {
        foreach (var directory in Directory.EnumerateDirectories(path))
        {
            DeleteDirectory(directory);
        }

        foreach (var fileName in Directory.EnumerateFiles(path))
        {
            var fileInfo = new FileInfo(fileName)
            {
                Attributes = FileAttributes.Normal
            };

            fileInfo.Delete();
        }

        Directory.Delete(path);
    }

    private void Fetch()
    {
        using var repo = new Repository(_providerOptions.LocalPath);
        FetchOptions options = new FetchOptions
        {
            CredentialsProvider = _credentialsHandler
        };

        foreach (var remote in repo.Network.Remotes)
        {
            var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);

            _logger.LogInformation("Fetching from remote {Name} at {Url}", remote.Name, remote.Url);

            Commands.Fetch(repo, remote.Name, refSpecs, options, string.Empty);
        }
    }

    private string GetRelativePath(string fullPath)
    {
        return Path.GetRelativePath(_providerOptions.LocalPath ?? "", fullPath).NormalizePathSeparators();
    }
}
