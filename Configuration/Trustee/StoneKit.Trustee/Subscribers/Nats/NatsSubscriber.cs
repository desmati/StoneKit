namespace Trustee.Subscribers;

using NATS.Client;

using Serilog;

using System;


/// <summary>
/// Implementation of <see cref="ISubscriber"/> for receiving messages from NATS.
/// </summary>
public class NatsSubscriber : ISubscriber
{
    private readonly Options _options;
    private static IConnection? _connection;

    /// <summary>
    /// Gets the name of the NATS subscriber.
    /// </summary>
    public string Name => "NATS";

    /// <summary>
    /// Initializes a new instance of the <see cref="NatsSubscriber"/> class.
    /// </summary>
    /// <param name="options">The options for the NATS connection.</param>
    public NatsSubscriber(Options options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Initializes the NATS subscriber.
    /// </summary>
    public void Initialize()
    {
        _options.AsyncErrorEventHandler += (sender, args) => { Log.Information("NATS replied with an error message: {Message}", args.Error); };
        _options.ClosedEventHandler += (sender, args) => { Log.Error(args.Error, "NATS connection was closed"); };
        _options.DisconnectedEventHandler += (sender, args) => { Log.Error(args.Error, "NATS connection was disconnected"); };
        _options.ReconnectedEventHandler += (sender, args) => { Log.Information("NATS connection was restored"); };

        var connectionFactory = new ConnectionFactory();
        _connection = connectionFactory.CreateConnection(_options);
    }

    /// <summary>
    /// Subscribes to messages on the specified NATS subject with the provided message handler.
    /// </summary>
    /// <param name="topic">The NATS subject to subscribe to.</param>
    /// <param name="handler">The action to be invoked when a message is received on the subscribed subject.</param>
    public void Subscribe(string topic, Action<string> handler)
    {
        Log.Information("Subscribing to NATS subject '{Subject}'", topic);

        _connection?.SubscribeAsync(topic, (sender, args) =>
        {
            Log.Information("Received subscription on NATS subject '{Subject}'", topic);

            var message = args.Message.ToString();

            handler(message);
        });

        Log.Information("Subscribed to NATS subject '{Subject}'", topic);
    }
}
