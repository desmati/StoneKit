# StoneKit DataStore

StoneKit DataStore is a lightweight, high-performance data storage library for .NET Core 8+ applications. 
It allows for simple and efficient storage and retrieval of objects by ID, with optional background cleanup of old files based on configurable settings.

## Installation

You can install StoneKit.DataStore via NuGet Package Manager:

```
PM> Install-Package StoneKit.DataStore
```

## Features

- **Simple Storage and Retrieval:**
  - Store and retrieve objects by ID.
  - Automatically generate IDs based on object content if not provided.

- **Background Cleanup:**
  - Configurable background task to delete old files based on a specified lifespan.

- **Asynchronous Operations:**
  - Utilizes asynchronous file operations for optimal performance.

- **Configuration Options:**
  - Configure storage path, file lifetime, and cleanup interval at application startup.

## Usage

### Setting up your project

1. **Configure Services:**

   In your `Program.cs`, use the `AddDataStore` extension method to configure the data store.

```csharp
using Microsoft.Extensions.DependencyInjection;
using SimpleDataStore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataStore(options =>
{
    options.DirectoryPath = Path.Combine(builder.Environment.ContentRootPath, "DataStore"); // Or null
    options.FileLifetime = TimeSpan.FromDays(7); // Or null
    options.CleanupInterval = TimeSpan.FromHours(1); // Or null
});

var app = builder.Build();

app.MapGet("/", async (DataStore dataStore) =>
{
    // Example data
    var data = new MyData { Content = "Example Data" };

    // Save the data
    var id = await dataStore.SaveAsync(data);

    // Retrieve the data
    var retrievedData = await dataStore.LoadAsync<MyData>(id);
    return Results.Ok(retrievedData);
});

app.Run();
```

2. **Data Model:**

   Define your data model.

   ```csharp
   public class MyData
   {
       /// <summary>
       /// Gets or sets the content of the data.
       /// </summary>
       public string Content { get; set; }
   }
   ```

## Configuration Options

You can configure the data store with the following options:

- **DirectoryPath:** Optional; The directory path where data will be stored.
- **FileLifetime:** Optional; The time span after which files should be considered for cleanup.
- **CleanupInterval:** Optional; The interval at which the cleanup task will run.

### Example Configuration

```csharp
builder.Services.AddDataStore(options =>
{
    options.DirectoryPath = Path.Combine(builder.Environment.ContentRootPath, "DataStore");
    options.FileLifetime = TimeSpan.FromDays(7);
    options.CleanupInterval = TimeSpan.FromHours(1);
});
```

## Example Usage

### Saving Data

```csharp
public class MyDataService
{
    private readonly DataStore _dataStore;

    public MyDataService(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public async Task<string> SaveDataAsync(MyData data)
    {
        return await _dataStore.SaveAsync(data);
    }
}
```

### Loading Data

```csharp
public class MyDataService
{
    private readonly DataStore _dataStore;

    public MyDataService(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public async Task<MyData?> LoadDataAsync(string id)
    {
        var result = await _dataStore.LoadAsync<MyData>(id);
        return result.HasValue ? result.Value : null;
    }
}
```

## Contributions

Contributions and feedback are welcome! Feel free to submit issues, feature requests, or pull requests on the [GitHub repository](https://github.com/desmati/StoneKit/).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.