namespace System.Caching
{
    public interface IStaticCache
    {
        Task AddAsync(string key, object value, TimeSpan duration);
        Task<T?> GetAsync<T>(string key);
        Task RemoveAsync(string key);
        Task ClearAsync();
    }
}
