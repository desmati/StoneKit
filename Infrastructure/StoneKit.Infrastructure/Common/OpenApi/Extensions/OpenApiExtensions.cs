using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Filters;

using System.Reflection;

namespace Microsoft.OpenApi
{
    public static class OpenApiExtensions
    {
        public static void ConfigureOpenApi(this WebApplicationBuilder builder, string? appName, OpenApiOptions? options, Assembly? assembly)
        {
            if (options?.Enabled != true)
            {
                return;
            }

            if (assembly == null)
            {
                return;
            }

            var anyEndpoints = (options.Endpoints?.Count ?? 0) > 0;

            builder.Services.AddSwaggerExamplesFromAssemblies([assembly]);

            builder.Services.AddSwaggerGen(c =>
            {
                if (anyEndpoints)
                {
                    foreach (var item in options.Endpoints!)
                    {
                        c.SwaggerDoc(item, new OpenApiInfo
                        {
                            Title = $"{appName} {item} API",
                            Version = "v2"
                        });
                    }
                }

                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();

                c.ExampleFilters();

                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.OperationFilter<AddUniqueHeaderOperationFilter>();

                try
                {
                    string xmlFile = $"{assembly?.GetName().Name}.xml";
                    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                }
                catch { }
            });
        }
    }

}

