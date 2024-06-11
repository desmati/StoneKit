namespace Trustee.Publishers;

using Microsoft.Extensions.Logging;

using RabbitMQ.Client;

using System.Text;

using Trustee.Host;


/// <summary>
/// Represents a publisher for sending messages to RabbitMQ.
/// </summary>
public class RabbitMqPublisher : IPublisher
{
    private readonly ILogger<RabbitMqPublisher> _logger;

    private readonly RabbitMqOptions _options;
    private string? _exchangeName;
    private static IModel? _channel;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqPublisher"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="options">The options for configuring the RabbitMQ connection.</param>
    public RabbitMqPublisher(ILogger<RabbitMqPublisher> logger, RabbitMqOptions options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Initializes the RabbitMQ publisher.
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

        connection.CallbackException += (sender, args) => { _logger.LogError(args.Exception, "RabbitMQ callback exception"); };
        connection.ConnectionBlocked += (sender, args) => { _logger.LogError("RabbitMQ connection is blocked. Reason: {Reason}", args.Reason); };
        connection.ConnectionShutdown += (sender, args) => { _logger.LogError("RabbitMQ connection was shut down. Reason: {ReplyText}", args.ReplyText); };
        connection.ConnectionUnblocked += (sender, args) => { _logger.LogInformation("RabbitMQ connection was unblocked"); };

        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout);

        _logger.LogInformation("RabbitMQ publisher initialized");
    }

    /// <summary>
    /// Publishes a message to RabbitMQ with the specified topic and message content.
    /// </summary>
    /// <param name="topic">The routing key or topic for the message.</param>
    /// <param name="message">The content of the message to be published.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Publish(string topic, string message)
    {
        _logger.LogInformation("Publishing message with routing key {RoutingKey}", topic);

        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(_exchangeName, topic, null, body);

        return Task.CompletedTask;
    }
}
