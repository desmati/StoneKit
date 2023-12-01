namespace System
{
    /// <summary>
    ///     Utility class for creating exceptions.
    /// </summary>
    public static class Error
    {
        /// <summary>
        /// Creates an ArgumentNullException with the specified parameter name.
        /// </summary>
        public static Exception ArgumentNull(string paramName)
        {
            return new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Creates an InvalidOperationException with the specified message.
        /// </summary>
        public static Exception InvalidOperation(string message, Exception? innerException = null)
        {
            return new InvalidOperationException(message, innerException);
        }

        /// <summary>
        /// Creates a NotImplementedException.
        /// </summary>
        public static Exception NotImplemented(string? message = null, Exception? innerException = null)
        {
            return new NotImplementedException(message, innerException);
        }

        /// <summary>
        /// Creates a NotSupportedException.
        /// </summary>
        public static Exception NotSupported(string? message = null, Exception? innerException = null)
        {
            return new NotSupportedException(message, innerException);
        }
    }

}
