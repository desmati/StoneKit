namespace Trustee.Host
{
    /// <summary>
    /// Exception that is thrown when a provider option is found to be null or empty.
    /// </summary>
    [Serializable]
    public class ProviderOptionNullException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderOptionNullException"/> class.
        /// </summary>
        public ProviderOptionNullException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderOptionNullException"/> class
        /// with a specified error message indicating the name of the null or empty provider option.
        /// </summary>
        /// <param name="name">The name of the provider option that is null or empty.</param>
        public ProviderOptionNullException(string? name)
            : base($"{name} cannot be NULL or empty.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderOptionNullException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ProviderOptionNullException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
