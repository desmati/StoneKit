namespace System.Net
{
    public class RequestConfiguration
    {
        public Dictionary<string, string>? Headers { get; set; }
        public string? Unique { get; set; }

        /// <summary>
        /// Either staring/ending with "/" or not
        /// </summary>
        public string? Uri { get; set; }
        public string? Query { get; set; }
    }
}
