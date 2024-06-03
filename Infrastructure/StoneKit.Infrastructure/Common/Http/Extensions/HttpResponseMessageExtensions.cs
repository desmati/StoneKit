using Newtonsoft.Json;

using Serilog;

namespace System.Net
{
    public static class HttpResponseMessageExtensions
    {
        public static T? As<T>(this HttpResponseMessage httpResponseMessage)
        {
            var responseString = httpResponseMessage.AsString();
            if (string.IsNullOrEmpty(responseString))
            {
                return default;
            }

            var obj = JsonConvert.DeserializeObject<T>(responseString, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });

            return obj;
        }

        public static string? AsString(this HttpResponseMessage httpResponseMessage)
        {
            var responseString = httpResponseMessage.Content.ReadAsStringAsync()?.GetAwaiter().GetResult();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                httpResponseMessage.Content?.Dispose();
                Log.Error("Http error occured. Code : {HttpResponseCode} , Response : {Response}", httpResponseMessage.StatusCode, responseString);
                return null;
            }

            return responseString;
        }
    }
}