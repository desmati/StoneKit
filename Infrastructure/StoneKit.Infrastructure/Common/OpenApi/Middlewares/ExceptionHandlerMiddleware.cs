using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using Serilog;

using StoneKit.Infrastructure.Common;

using System.Net;

namespace Microsoft.Extensions.Configuration
{
    public sealed class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ExceptionHandlerMiddleware(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception exception)
            {
                try
                {
                    UriBuilder uriBuilder = new UriBuilder
                    {
                        Scheme = httpContext.Request.Scheme,
                        Host = httpContext.Request.Host.Host,
                        Path = httpContext.Request.Path.Value,
                        Query = httpContext.Request.QueryString.Value
                    };
                    if (httpContext.Request.Host.Port.HasValue)
                    {
                        uriBuilder.Port = httpContext.Request.Host.Port.Value;
                    }

                    if (httpContext.Response.HasStarted)
                    {
                        Log.Error(exception, "The response has already started, the error handler will not be executed.");
                        throw;
                    }


                    var operationResult = new OperationResult<ExceptionDetailsModel>
                    {
                        ResponseInfo = new ResponseInfo()
                    };
                    if (exception is ApiException apiException)
                    {
                        Log.Write(apiException.LogLevel.ConvertLogLevelFromMicrosoft(), exception,
                            "Business exception has occured. Path : {Path} , HttpMethod : {HttpMethod} , RemoteIp : {RemoteIp}",
                            uriBuilder.Uri.AbsoluteUri,
                            httpContext.Request.Method,
                            httpContext.Connection?.RemoteIpAddress?.ToString());

                        if (exception is UserFriendlyException userFriendlyException)
                        {
                            httpContext.Response.StatusCode = 200; //Ok    
                        }

                        operationResult.ResponseInfo.ErrorCode = apiException?.ErrorCode?.ToString() ?? "NONE";
                    }
                    else
                    {
                        Log.Error(exception,
                            "Error occured. Path : {Path} , HttpMethod : {HttpMethod} , RemoteIp : {RemoteIp}",
                            uriBuilder.Uri.AbsoluteUri,
                            httpContext.Request.Method,
                            httpContext.Connection?.RemoteIpAddress?.ToString());

                        operationResult.Data = exception.GetFullExceptionDetails();
                    }

                    operationResult.ResponseInfo.Message = exception.GetExceptionMessages();
                    operationResult.ResponseInfo.Success = false;

                    Formatting jsonFormatting = _hostingEnvironment.IsDevelopment() ? Formatting.Indented : Formatting.None;
                    string json = JsonConvert.SerializeObject(operationResult, new JsonSerializerSettings { Formatting = jsonFormatting });

                    httpContext.Response.ContentType = Constants.JsonContentType;

                    await httpContext.Response.WriteAsync(json);
                }
                catch (Exception exception2)
                {
                    Log.Fatal(exception2,
                        $"Couldn't handle exception. {Environment.NewLine}. {exception.GetExceptionMessages()} ------- {exception2.GetExceptionMessages()}");
                }
            }
        }
    }
}