namespace Trustee.Subscribers;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Serilog;

using System.Text;

/// <summary>
/// Represents a subscriber for RabbitMQ.
/// </summary>
public class RabbitMqSubscriber : ISubscriber
{
    private readonly RabbitMqOptions _options;
    private string? _exchangeName;
    private static IModel? _channel;

    /// <summary>
    /// Gets the name of the subscriber.
    /// </summary>
    public string Name => "RabbitMQ";

    /// <summary>
    /// Initializes a new instance of the RabbitMqSubscriber class with the specified options.
    /// </summary>
    /// <param name="options">The options for configuring RabbitMQ connection settings.</param>
    public RabbitMqSubscriber(RabbitMqOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Initializes the RabbitMQ subscriber.
    /// </summary>
    public void Initialize()
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            VirtualHost = _options.VirtualHost,
            UserName = _options.UserName,
            Password = _options.Password
        };

        _exchangeName = _options.ExchangeName;

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        connection.CallbackException += (sender, args) => { Log.Error(args.Exception, "RabbitMQ callback exception"); };

        connection.ConnectionBlocked += (sender, args) => { Log.Error("RabbitMQ connection is blocked. Reason: {Reason}", args.Reason); };

        connection.ConnectionShutdown += (sender, args) => { Log.Error("RabbitMQ connection was shut down. Reason: {ReplyText}", args.ReplyText); };

        connection.ConnectionUnblocked += (sender, args) => { Log.Information("RabbitMQ connection was unblocked"); };

        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout);
    }

    /// <summary>
    /// Subscribes to a topic in RabbitMQ.
    /// </summary>
    /// <param name="topic">The topic to subscribe to.</param>
    /// <param name="handler">The action to handle incoming messages.</param>
    public void Subscribe(string topic, Action<string> handler)
    {
        Log.Information("Binding to RabbitMQ queue with routing key '{RoutingKey}'", topic);

        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queueName, _exchangeName, topic);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, args) =>
        {
            Log.Information("Received message with routing key '{RoutingKey}'", args.RoutingKey);

            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            handler(message);
        };

        var consumerTag = _channel.BasicConsume(queueName, true, consumer);

        Log.Information("Consuming RabbitMQ queue {QueueName} for consumer '{ConsumerTag}'", queueName, consumerTag);
    }
}
