using Newtonsoft.Json;

using System.Reflection;
using System.Text;

namespace System.Net
{
    public static class ExceptionExtensions
    {
        public static string GetExceptionMessages(this Exception? exception)
        {
            StringBuilder stringBuilder = new StringBuilder(150);
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                exception = exception?.InnerException;
            }

            return stringBuilder.ToString();
        }

        public static string GetExceptionMessagesWithPrefixTypes(this Exception? exception)
        {
            StringBuilder stringBuilder = new StringBuilder(200);
            while (exception != null)
            {
                stringBuilder.AppendLine($"{exception.GetType().FullName} {exception.Message}");
                exception = exception?.InnerException;
            }

            return stringBuilder.ToString();
        }

        public static string GetExceptionTypes(this Exception? exception)
        {
            StringBuilder stringBuilder = new StringBuilder(100);
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.GetType().FullName);
                exception = exception?.InnerException;
            }

            return stringBuilder.ToString();
        }

        public static string GetExceptionStacktrace(this Exception? exception)
        {
            StringBuilder stringBuilder = new StringBuilder(2500);
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.StackTrace);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
                exception = exception?.InnerException;
            }

            return stringBuilder.ToString();
        }

        public static string GetExceptionStacktraceWithPrefixTypes(this Exception? exception)
        {
            StringBuilder stringBuilder = new StringBuilder(2500);
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.GetType().FullName + " :");
                stringBuilder.AppendLine(exception.StackTrace);
                stringBuilder.AppendLine();
                exception = exception?.InnerException;
            }

            return stringBuilder.ToString();
        }

        public static string GetExceptionUniqueProperties(this Exception? exception)
        {
            StringBuilder stringBuilder = new StringBuilder(100);
            while (exception != null && exception.GetType() != typeof(Exception))
            {
                var exception1 = exception;
                PropertyInfo[] propertyInfos = exception.GetType().GetTypeInfo().GetProperties().Where(w => w.DeclaringType == exception1.GetType()).ToArray();
                var dictionary = propertyInfos.ToDictionary(d => d.Name, d => d.GetValue(exception1));
                if (dictionary?.Any() == true)
                {
                    stringBuilder.AppendLine(JsonConvert.SerializeObject(dictionary,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine();
                }

                exception = exception?.InnerException;
            }

            string data = stringBuilder.ToString();
            return string.IsNullOrWhiteSpace(data) ? "" : data;
        }

        public static ExceptionDetailsModel GetFullExceptionDetails(this Exception exception)
        {
            return new ExceptionDetailsModel
            {
                Messages = GetExceptionMessages(exception),
                Types = GetExceptionTypes(exception),
                Stacktrace = GetExceptionStacktrace(exception),
                UniqueProperties = GetExceptionUniqueProperties(exception)
            };
        }

        public static string GetFullExceptionDetailsJson(this Exception exception)
        {
            return JsonConvert.SerializeObject(new
            {
                Messages = GetExceptionMessages(exception),
                Types = GetExceptionTypes(exception),
                Stacktraces = GetExceptionStacktrace(exception),
                UniqueProperties = GetExceptionUniqueProperties(exception)
            });
        }
    }
}
