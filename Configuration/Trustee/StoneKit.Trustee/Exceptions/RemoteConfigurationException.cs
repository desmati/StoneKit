﻿namespace Trustee.Client;

using System;

/// <summary>
/// Represents an exception that is thrown when there is an error in the remote configuration.
/// </summary>
[Serializable]
public class RemoteConfigurationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the RemoteConfigurationException class.
    /// </summary>
    public RemoteConfigurationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the RemoteConfigurationException class with a specified name.
    /// </summary>
    /// <param name="name">The name of the configuration item.</param>
    public RemoteConfigurationException(string name)
        : base($"{name} cannot be NULL or empty.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the RemoteConfigurationException class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public RemoteConfigurationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
