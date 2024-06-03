namespace StoneKit.Infrastructure.Common
{
    internal class Constants
    {
        public const string JsonContentType = "application/json";
        public const string UniqueHeader = "unique";

    }

    public static class EnvironmentName
    {
        public const string Development = nameof(Development);
        public const string Staging = nameof(Staging);
        public const string Production = nameof(Production);
    }

}
