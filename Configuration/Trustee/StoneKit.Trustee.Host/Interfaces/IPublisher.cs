namespace Trustee.Publishers;

/// <summary>
/// Defines the contract for a publisher that publishes messages to a topic.
/// </summary>
public interface IPublisher
{
    /// <summary>
    /// Initializes the publisher.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Publishes a message to the specified topic.
    /// </summary>
    /// <param name="topic">The topic to which the message will be published.</param>
    /// <param name="message">The message to be published.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    Task Publish(string topic, string message);
}
