namespace System.Net
{

    public class HttpServiceOptions
    {
        public bool IsLive { get; set; } = false;
        public string? Endpoint { set; get; }

        public int TimeoutSeconds { set; get; }

        public string? ProxyHost { get; set; }
        public int? ProxyPort { get; set; }
        public string? ProxyUser { get; set; }
        public string? ProxyPassword { get; set; }
        public bool UseProxy { get; set; }


    }
}
