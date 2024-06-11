namespace Trustee.Publishers;

using Microsoft.Extensions.Logging;

using NATS.Client;

using System;
using System.Text;
using System.Threading.Tasks;

using Trustee.Host;

/// <summary>
/// Represents a configuration publisher that communicates with NATS.
/// </summary>
public class NatsPublisher : IPublisher
{
    private readonly ILogger<NatsPublisher> _logger;

    private readonly Options _options;
    private static IConnection _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="NatsPublisher"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging messages.</param>
    /// <param name="options">The NATS connection options.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is null.
    /// </exception>
    public NatsPublisher(ILogger<NatsPublisher> logger, Options options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Initializes the NATS publisher by setting up event handlers and creating a connection.
    /// </summary>
    public void Initialize()
    {
        // Set up event handlers for NATS connection events
        _options.AsyncErrorEventHandler += (sender, args) => { _logger.LogError("NATS replied with an error message: {Message}", args.Error); };
        _options.ClosedEventHandler += (sender, args) => { _logger.LogError(args.Error, "NATS connection was closed"); };
        _options.DisconnectedEventHandler += (sender, args) => { _logger.LogError(args.Error, "NATS connection was disconnected"); };
        _options.ReconnectedEventHandler += (sender, args) => { _logger.LogInformation("NATS connection was restored"); };

        // Create NATS connection
        var connectionFactory = new ConnectionFactory();
        _connection = connectionFactory.CreateConnection(_options);

        _logger.LogInformation("NATS publisher initialized");
    }

    /// <summary>
    /// Publishes a message to NATS with the specified subject.
    /// </summary>
    /// <param name="topic">The subject/topic to publish the message to.</param>
    /// <param name="message">The message content to publish.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task Publish(string topic, string message)
    {
        _logger.LogInformation("Publishing message to NATS with subject {Subject}", topic);

        var data = Encoding.UTF8.GetBytes(message);
        _connection.Publish(topic, data);

        return Task.CompletedTask;
    }
}
