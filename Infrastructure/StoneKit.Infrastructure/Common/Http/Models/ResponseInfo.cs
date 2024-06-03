namespace System.Net
{
    public sealed class ResponseInfo
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public string ErrorCode { get; set; } = "";

        public List<string> Warnings { get; set; } = new List<string>();
    }
}
