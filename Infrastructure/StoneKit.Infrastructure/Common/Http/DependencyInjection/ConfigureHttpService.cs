using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Net;

namespace Microsoft.AspNetCore.Mvc
{
    public static class HttpServiceExtensions
    {
        private const string PATH_HttpServiceOptions = @"Assets\Settings\http.json";

        internal static void ConfigureHttpService(this WebApplicationBuilder builder)
        {
            if (File.Exists(PATH_HttpServiceOptions))
            {
                builder.Configuration.AddJsonFile(PATH_HttpServiceOptions, false, true);

                var httpSection = builder.Configuration.GetSection("http");
                var httpServiceOptions = httpSection?.Get<HttpServiceOptions>();

                if (httpServiceOptions != null && httpSection != null)
                {
                    builder.Services.AddSingleton(httpServiceOptions);
                    builder.Services.Configure<HttpServiceOptions>(httpSection);

                    builder.Services.AddSingleton<HttpService>();
                    builder.Services.AddTransient<IHttpService, HttpService>();
                }
            }
        }
    }
}
