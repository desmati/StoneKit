using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using StoneKit.Infrastructure.Common;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.OpenApi
{
    public sealed class AddUniqueHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Required = true,
                Name = Constants.UniqueHeader,
                In = ParameterLocation.Header,
                Description = "Unique http header from client.",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString(Guid.NewGuid().ToString())
                }
            });
        }
    }
}
