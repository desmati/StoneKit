using Microsoft.Extensions.Logging;

namespace System.Net
{
    public sealed class UserFriendlyException : ApiException
    {
        public UserFriendlyException(string message) : base(message)
        {
            LogLevel = LogLevel.Warning;
        }
    }
}
