namespace Trustee.Host;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Hosted service responsible for managing the configuration service.
/// </summary>
public class HostedConfigurationService : IHostedService, IDisposable
{
    private readonly ILogger<HostedConfigurationService> _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IConfigurationService _configurationService;

    private Task? _executingTask;
    private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="HostedConfigurationService"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="applicationLifetime">The host application lifetime.</param>
    /// <param name="configurationService">The configuration service.</param>
    public HostedConfigurationService(ILogger<HostedConfigurationService> logger, IHostApplicationLifetime applicationLifetime, IConfigurationService configurationService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
    }

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Configuration Service");

        _applicationLifetime.ApplicationStarted.Register(OnStarted);
        _applicationLifetime.ApplicationStopping.Register(OnStopping);
        _applicationLifetime.ApplicationStopped.Register(OnStopped);

        _executingTask = ExecuteAsync(_stoppingCts.Token);

        if (_executingTask.IsCompleted)
        {
            return _executingTask;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_executingTask == null)
        {
            return;
        }

        try
        {
            // Signal cancellation to the executing method
            _stoppingCts.Cancel();
        }
        finally
        {
            // Wait until the task completes or the stop token triggers
            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }

    /// <summary>
    /// Executes the main logic of the configuration service asynchronously.
    /// </summary>
    private async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _configurationService.Initialize(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred while attempting to initialize the configuration provider");

            _logger.LogInformation("The application will be terminated");

            await StopAsync(stoppingToken);
            _applicationLifetime.StopApplication();
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the resources used by the hosted service.
    /// </summary>
    /// <param name="disposing">Whether the method is called from the <see cref="Dispose"/> method or a finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _stoppingCts.Cancel();
        }

        _disposed = true;
    }

    /// <summary>
    /// Event handler for the application started event.
    /// </summary>
    private void OnStarted()
    {
        _logger.LogInformation("Configuration Service started");
    }

    /// <summary>
    /// Event handler for the application stopping event.
    /// </summary>
    private void OnStopping()
    {
        _logger.LogInformation("Configuration Service is stopping...");
    }

    /// <summary>
    /// Event handler for the application stopped event.
    /// </summary>
    private void OnStopped()
    {
        _logger.LogInformation("Configuration Service stopped");
    }
}
