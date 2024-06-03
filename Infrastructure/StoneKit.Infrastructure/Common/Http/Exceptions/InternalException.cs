using Microsoft.Extensions.Logging;

namespace System.Net
{
    public sealed class InternalException : ApiException
    {
        public InternalException(string message) : base(message)
        {
            LogLevel = LogLevel.Error;
        }
    }
}
