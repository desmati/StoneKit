namespace Microsoft.Extensions.Configuration
{
    public sealed class OpenApiOptions
    {
        public bool Enabled { get; } = true;

        /// <summary>
        /// e.g. /assets/themes/theme-material.css
        /// </summary>
        public string? CustomThemeCssPath { get; } = null;
        public IList<string>? Endpoints { get; set; } = null;
    }
}
