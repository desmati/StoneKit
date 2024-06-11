namespace Trustee.Publishers;

using Microsoft.Extensions.Logging;

using StackExchange.Redis;

using System;
using System.IO;
using System.Threading.Tasks;

using Trustee.Host;


/// <summary>
/// Publisher responsible for publishing configuration updates to Redis.
/// </summary>
public class RedisPublisher : IPublisher
{
    private readonly ILogger<RedisPublisher> _logger;

    private readonly ConfigurationOptions _options;
    private static IConnectionMultiplexer? _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisPublisher"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">The Redis configuration options.</param>
    public RedisPublisher(ILogger<RedisPublisher> logger, ConfigurationOptions configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Initializes the Redis publisher.
    /// </summary>
    public void Initialize()
    {
        using (var writer = new StringWriter())
        {
            _connection = ConnectionMultiplexer.Connect(_options, writer);

            _logger.LogTrace("Redis publisher connected with log:\r\n{Log}", writer);
        }

        _connection.ErrorMessage += (sender, args) => { _logger.LogError("Redis replied with an error message: {Message}", args.Message); };
        _connection.ConnectionFailed += (sender, args) => { _logger.LogError(args.Exception, "Redis connection failed"); };
        _connection.ConnectionRestored += (sender, args) => { _logger.LogInformation("Redis connection restored"); };

        _logger.LogInformation("Redis publisher initialized");
    }

    /// <summary>
    /// Publishes a message to a specified channel in Redis.
    /// </summary>
    /// <param name="topic">The channel to publish to.</param>
    /// <param name="message">The message to publish.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Publish(string topic, string message)
    {
        if (topic == null)
        {
            throw new ArgumentNullException(nameof(topic));
        }

        if (_connection == null)
        {
            throw new ArgumentNullException(nameof(IConnectionMultiplexer));
        }

        _logger.LogInformation("Publishing message to channel {Channel}", topic);

        var publisher = _connection.GetSubscriber();

        var clientCount = await publisher.PublishAsync(topic, message);

        _logger.LogInformation("Message to channel {Channel} was received by {ClientCount} clients", topic, clientCount);
    }
}
