namespace Trustee.Subscribers;

/// <summary>
/// Represents a subscriber interface for receiving messages on a specific topic.
/// </summary>
public interface ISubscriber
{
    /// <summary>
    /// Gets the name of the subscriber.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Initializes the subscriber.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Subscribes to messages on the specified topic with the provided message handler.
    /// </summary>
    /// <param name="topic">The topic to subscribe to.</param>
    /// <param name="handler">The action to be invoked when a message is received on the subscribed topic.</param>
    void Subscribe(string topic, Action<string> handler);
}
