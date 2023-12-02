namespace System.Reflection.Mapping
{
    /// <summary>
    /// Represents an exception that occurs during mapping or binding in the Transverse library.
    /// </summary>
    public class TransverseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseException"/> class.
        /// </summary>
        public TransverseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public TransverseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public TransverseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
