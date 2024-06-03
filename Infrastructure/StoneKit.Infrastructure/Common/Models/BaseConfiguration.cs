using Microsoft.AspNetCore.Cors.Infrastructure;

using System.Reflection;

namespace Microsoft.Extensions.Configuration
{
    public abstract class BaseConfiguration
    {
        public string? AppName { get; set; }
        public string? AppUrl { get; set; }
        public Assembly? ValidatorAssembly { get; set; }
        public OpenApiOptions? OpenApiOptions { get; set; }
        public SerilogOptions? SerilogOptions { get; set; }

        public Action<CorsPolicyBuilder>? Cors { get; set; }

        public Action? InitializeMiddlewareAction { get; set; }
        public bool? UseFluentValidators { get; set; } = false;
    }
}
