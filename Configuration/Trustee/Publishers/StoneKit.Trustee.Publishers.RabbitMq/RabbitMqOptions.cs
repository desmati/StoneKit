namespace Trustee.Publishers;

/// <summary>
/// Represents the options for configuring RabbitMQ connection.
/// </summary>
public class RabbitMqOptions
{
    /// <summary>
    /// Gets or sets the host name of the RabbitMQ server.
    /// Defaults to "localhost".
    /// </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// Gets or sets the virtual host to access during the connection.
    /// Defaults to "/".
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Gets or sets the username for authenticating to the RabbitMQ server.
    /// Defaults to "guest".
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// Gets or sets the password for authenticating to the RabbitMQ server.
    /// Defaults to "guest".
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Gets or sets the name of the fanout exchange.
    /// Defaults to "configuration-service".
    /// </summary>
    public string ExchangeName { get; set; } = "configuration-service";
}
