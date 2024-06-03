using Microsoft.AspNetCore.Http;

using Serilog;

namespace Microsoft.Extensions.Hosting
{
    public sealed class InitializeMiddleware : IMiddleware
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private static bool _initialized;
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        public InitializeMiddleware(IHostApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_initialized)
            {
                await next(context);
                return;
            }

            await SemaphoreSlim.WaitAsync();

            if (_initialized)
            {
                await next(context);
                return;
            }

            try
            {
                _initialized = true;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Core Initialization Failed.");
                _applicationLifetime.StopApplication();
                return;
            }
            finally
            {
                SemaphoreSlim.Release();
            }

            await next(context);
        }
    }
}