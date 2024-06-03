using Serilog.AspNetCore;

namespace Microsoft.Extensions.Configuration
{
    public sealed class SerilogOptions
    {
        public Action<RequestLoggingOptions>? Options = null;
        public bool UseSerilog { get; set; } = true;
    }
}
