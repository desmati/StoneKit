using Newtonsoft.Json;

using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace System.Net
{
    public class HttpService : IHttpService
    {
        public string accept_Language { get; set; } = "en";
        private readonly HttpServiceOptions _config;

        /// <summary>
        /// Creates a new instance of ClientService to access API actions
        /// </summary>
        public HttpService(HttpServiceOptions config)
        {
            _config = config;

            if (config == null)
            {
                throw new Exception("Configure http section for the HttpServiceOptions");
            }
        }

        private HttpClient CreateClient()
        {
            var handler = new HttpClientHandler();

            if (_config.UseProxy && !string.IsNullOrEmpty(_config.ProxyHost))
            {
                handler.UseProxy = true;
                handler.Proxy = (_config.ProxyPort ?? 0) > 0
                    ? new WebProxy(_config.ProxyHost, _config.ProxyPort!.Value)
                    : new WebProxy(_config.ProxyHost);
                if (!string.IsNullOrEmpty(_config.ProxyUser))
                {
                    handler.DefaultProxyCredentials = new NetworkCredential(_config.ProxyUser, _config.ProxyPassword);
                }
            }

            var client = new HttpClient(handler);
            client.BaseAddress = new Uri(_config.Endpoint!);
            client.Timeout = new TimeSpan(0, 0, _config.TimeoutSeconds);

            return client;
        }


        #region Private Methods

        private CancellationToken StoppingToken()
        {
            return new CancellationTokenSource(TimeSpan.FromSeconds(_config.TimeoutSeconds)).Token;
        }

        private string GetEndpoint(string? uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new Exception("Invalid URL!");
            }
            return $"/{uri}";
        }

        public async Task<TResult?> GetAsync<TResult>(RequestConfiguration request, CancellationToken cancellationToken)
        {
            var endpoint = GetEndpoint(request.Uri);
            var urlBuilder_ = new StringBuilder();
            urlBuilder_
                 .Append(_config.Endpoint!.TrimEnd('/'))
                 .Append($"/{endpoint.TrimStart('/').TrimEnd('/')}/");

            if (!string.IsNullOrEmpty(request.Query))
            {
                //urlBuilder_.Append(Uri.EscapeDataString("query") + "=").Append(Uri.EscapeDataString(query)).Append("&");
                urlBuilder_.Append($"{request.Query.TrimStart('/')}");
            }

            var client_ = CreateClient();

            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    if (accept_Language != null)
                    {
                        request_.Headers.TryAddWithoutValidation("Accept-Language", accept_Language);
                    }

                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<TResult?>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_;
                        }
                        else
                        if (status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_ ?? "NO RESPONSE DATA", headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        public async Task<TResult?> PostAsync<TResult>(RequestConfiguration request, object body, CancellationToken cancellationToken)
        {
            var endpoint = GetEndpoint(request.Uri);
            var urlBuilder_ = new StringBuilder();
            urlBuilder_
                 .Append($"{_config.Endpoint!.TrimEnd('/')}/")
                 .Append($"{endpoint.TrimEnd('/').TrimStart('/')}/")
                 .Append($"{request.Query?.TrimEnd('/').TrimStart('/')}");

            var client_ = CreateClient();
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    if (accept_Language != null)
                    {
                        request_.Headers.TryAddWithoutValidation("Accept-Language", accept_Language);
                    }
                    var content_ = body != null
                         ? new StringContent(JsonConvert.SerializeObject(body, new JsonSerializerSettings()))
                         : new StringContent(string.Empty, Encoding.UTF8, "text/plain");
                    content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
                    request_.Content = content_;
                    request_.Method = new HttpMethod("POST");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<TResult>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_;
                        }
                        else
                        if (status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_ ?? "NO RESPONSE DATA", headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!_config.IsLive && ex != null)
                {
                    throw new ApiException(ex);
                }
            }
            finally
            {
            }
            return default;
        }

        protected virtual async Task<T?> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers)
        {
            if (response == null || response.Content == null)
            {
                return default;
            }

            var ReadResponseAsString = false;
            if (ReadResponseAsString)
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    var typedBody = JsonConvert.DeserializeObject<T>(responseText, new JsonSerializerSettings());
                    return default;
                }
                catch (JsonException exception)
                {
                    var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
                }
            }
            else
            {
                try
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var streamReader = new StreamReader(responseStream))
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var serializer = JsonSerializer.Create(new JsonSerializerSettings());
                        var typedBody = serializer.Deserialize<T>(jsonTextReader);
                        return default;
                    }
                }
                catch (JsonException exception)
                {
                    var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                }
            }
        }

        void PrepareRequest(HttpClient client, RequestConfiguration config)
        {
            try { client.DefaultRequestHeaders.Remove("unique"); } catch { }
            client.DefaultRequestHeaders.Add("unique", config.Unique ?? Guid.NewGuid().ToString());

            try { client.DefaultRequestHeaders.Remove("credential"); } catch { }

            try { client.DefaultRequestHeaders.Remove("Authorization"); } catch { }

            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", accept_Language);

            if (config?.Headers?.Any() == true)
            {
                foreach (var item in config.Headers)
                {
                    try { client.DefaultRequestHeaders.Remove(item.Key); } catch { }
                    client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                }
            }

        }



        #endregion
    }
}
