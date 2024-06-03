namespace System.Net
{
    public interface IHttpService
    {
        string accept_Language { get; set; }

        Task<TResult?> GetAsync<TResult>(RequestConfiguration request, CancellationToken cancellationToken);
        Task<TResult?> PostAsync<TResult>(RequestConfiguration request, object body, CancellationToken cancellationToken);
    }
}