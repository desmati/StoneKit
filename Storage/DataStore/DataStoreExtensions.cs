using Microsoft.Extensions.DependencyInjection;

namespace System.Data;

/// <summary>
/// Provides extension methods to configure and add the data store to the service collection.
/// </summary>
public static class DataStoreExtensions
{
    /// <summary>
    /// Adds the data store to the service collection with the specified options.
    /// </summary>
    /// <typeparam name="T">The type of objects to store.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">The action to configure the data store options.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDataStore<T>(this IServiceCollection services, Action<DataStoreOptions> configureOptions) where T : class
    {
        var options = new DataStoreOptions();
        configureOptions(options);

        var dataStore = new DataStore<T>(options.DirectoryPath, options.FileLifetime, options.CleanupInterval);
        services.AddSingleton(dataStore);

        return services;
    }
}
