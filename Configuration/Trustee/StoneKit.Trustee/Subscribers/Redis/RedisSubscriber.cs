namespace Trustee.Subscribers;

using Serilog;

using StackExchange.Redis;

using RedisOptions = StackExchange.Redis.ConfigurationOptions;

/// <summary>
/// Represents a subscriber for Redis.
/// </summary>
public class RedisSubscriber : ISubscriber
{
    private readonly RedisOptions _options;
    private static ConnectionMultiplexer? _connection;

    /// <summary>
    /// Gets the name of the subscriber.
    /// </summary>
    public string Name => "Redis";

    /// <summary>
    /// Initializes a new instance of the RedisSubscriber class with the specified Redis configuration.
    /// </summary>
    /// <param name="configuration">The Redis configuration string.</param>
    public RedisSubscriber(string configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        _options = RedisOptions.Parse(configuration);

    }

    /// <summary>
    /// Initializes a new instance of the RedisSubscriber class with the specified Redis configuration options.
    /// </summary>
    /// <param name="configurationOptions">The Redis configuration options.</param>
    public RedisSubscriber(RedisOptions configurationOptions)
    {
        _options = configurationOptions ?? throw new ArgumentNullException(nameof(configurationOptions));
    }

    /// <summary>
    /// Initializes the Redis subscriber.
    /// </summary>
    public void Initialize()
    {
        using (var writer = new StringWriter())
        {
            _connection = ConnectionMultiplexer.Connect(_options, writer);

            Log.Information("Redis subscriber connected with log:\r\n{Log}", writer);
        }

        _connection.ErrorMessage += (sender, args) => { Log.Error("Redis replied with an error message: {Message}", args.Message); };

        _connection.ConnectionFailed += (sender, args) => { Log.Error(args.Exception, "Redis connection failed"); };

        _connection.ConnectionRestored += (sender, args) => { Log.Information("Redis connection restored"); };
    }

    /// <summary>
    /// Subscribes to a topic in Redis.
    /// </summary>
    /// <param name="topic">The topic to subscribe to.</param>
    /// <param name="handler">The action to handle incoming messages.</param>
    public void Subscribe(string? topic, Action<string>? handler)
    {
        if (string.IsNullOrEmpty(topic))
        {
            throw new ArgumentNullException(nameof(topic));
        }
        Log.Information("Subscribing to Redis channel '{Channel}'", topic);

        var channel = new RedisChannel(topic, RedisChannel.PatternMode.Literal);

        var subscriber = _connection?.GetSubscriber();

        subscriber?.Subscribe(channel, (redisChannel, value) =>
        {
            Log.Information("Received subscription on Redis channel '{Channel}'", topic);

            if (handler != null && value.HasValue)
            {
                handler(value!);
            }
        });

        var endpoint = subscriber?.SubscribedEndpoint(channel);
        Log.Information("Subscribed to Redis endpoint {Endpoint} for channel '{Channel}'", endpoint, topic);
    }
}
