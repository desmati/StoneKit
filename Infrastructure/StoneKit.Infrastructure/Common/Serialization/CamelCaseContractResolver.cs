namespace Newtonsoft.Json.Serialization
{
    public sealed class CamelCaseContractResolver : DefaultContractResolver
    {
        public CamelCaseContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy();
        }
    }
}
