namespace Trustee.Subscribers;

/// <summary>
/// Options for configuring RabbitMQ connection settings.
/// </summary>
public class RabbitMqOptions
{
    /// <summary>
    /// Gets or sets the host to connect to. Defaults to "localhost".
    /// </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// Gets or sets the virtual host to access during this connection. Defaults to "/".
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Gets or sets the username to use when authenticating to the server. Defaults to "guest".
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// Gets or sets the password to use when authenticating to the server. Defaults to "guest".
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Gets or sets the name of the fanout exchange. Defaults to "configuration-service".
    /// </summary>
    public string ExchangeName { get; set; } = "configuration-service";
}
