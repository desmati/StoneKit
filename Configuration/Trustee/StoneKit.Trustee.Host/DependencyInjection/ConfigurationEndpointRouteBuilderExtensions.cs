namespace Trustee.Host;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Trustee.Providers;


/// <summary>
/// Provides extension methods for configuring endpoints related to the ConfigurationService.
/// </summary>
public static partial class ConfigurationEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps endpoints for the ConfigurationService to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> used to map endpoints.</param>
    /// <param name="pattern">The URL pattern for the ConfigurationService endpoints. Defaults to "/configuration".</param>
    /// <returns>An <see cref="IEndpointConventionBuilder"/> that can be used to further configure the mapped endpoints.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="endpoints"/> is null.</exception>
    public static IEndpointConventionBuilder MapConfigurationService(this IEndpointRouteBuilder endpoints, string pattern = "/configuration")
    {
        if (endpoints == null)
        {
            throw new ArgumentNullException(nameof(endpoints));
        }

        if (pattern == null)
        {
            throw new ArgumentNullException(nameof(pattern));
        }

        var conventionBuilders = new List<IEndpointConventionBuilder>();

        var listConfigurationBuilder = endpoints.RegisterListRoute(pattern);
        conventionBuilders.Add(listConfigurationBuilder);

        var fileConfigurationBuilder = endpoints.RegisterFileRoute(pattern);
        conventionBuilders.Add(fileConfigurationBuilder);

        return new CompositeEndpointConventionBuilder(conventionBuilders);
    }

    /// <summary>
    /// Registers an endpoint to list configuration files.
    /// </summary>
    private static IEndpointConventionBuilder RegisterListRoute(this IEndpointRouteBuilder endpointRouteBuilder, string pattern)
    {
        var provider = endpointRouteBuilder.ServiceProvider.GetService<IProvider>();

        return endpointRouteBuilder.MapGet(pattern, async context =>
        {
            var files = await provider.ListPaths();

            context.Response.OnStarting(async () =>
            {
                await JsonSerializer.SerializeAsync(context.Response.Body, files);
            });

            context.Response.ContentType = "application/json; charset=UTF-8";
            await context.Response.Body.FlushAsync();
        });
    }

    /// <summary>
    /// Registers an endpoint to retrieve a specific configuration file.
    /// </summary>
    private static IEndpointConventionBuilder RegisterFileRoute(this IEndpointRouteBuilder endpointRouteBuilder, string pattern)
    {
        var provider = endpointRouteBuilder.ServiceProvider.GetService<IProvider>();

        return endpointRouteBuilder.MapGet(pattern + "/{name}", async context =>
        {
            var name = context.GetRouteValue("name")?.ToString();
            name = WebUtility.UrlDecode(name);

            var bytes = await provider.GetConfiguration(name);

            if (bytes == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            var fileContent = Encoding.UTF8.GetString(bytes);

            await context.Response.WriteAsync(fileContent);
            await context.Response.Body.FlushAsync();
        });
    }
}
