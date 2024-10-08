﻿using Microsoft.Extensions.Logging;

namespace System.Net
{
    public class ApiException : Exception
    {
        public OperationStatusCodes StatusCode { get; private set; }

        public string? Response { get; private set; }

        public IReadOnlyDictionary<string, IEnumerable<string>>? Headers { get; private set; }

        public LogLevel LogLevel { get; set; } = LogLevel.Error;
        public string ErrorCode => StatusCode.ToString();

        public ApiException(string message) : base(message)
        {
        }

        public ApiException(string message, OperationStatusCodes statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>>? headers, Exception? innerException)
             : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + response.Substring(0, response.Length >= 512 ? 512 : response.Length), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public ApiException(Exception innerException)
             : base(innerException.Message, innerException)
        {
            StatusCode = 0;
            Response = "NO RESPONSE DATA";
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    public class ApiException<TResult> : ApiException
    {
        public TResult Result { get; private set; }

        public ApiException(string message, OperationStatusCodes statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, Exception innerException)
             : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }
}
