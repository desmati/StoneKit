# StoneKit Infrastructure

StoneKit Infrastructure is a comprehensive boilerplate for starting new ASP.NET Core 8 API projects. 
It integrates essential libraries and services, including FluentValidation, Serilog logging, hashing, HttpClient, OpenAPI documentation, and more, to streamline the setup and development process.

## Installation

You can install StoneKit.Infrastructure via NuGet Package Manager:

```
PM> Install-Package StoneKit.Infrastructure
```

## Features

- **Logging:**
  - Integrates Serilog for robust and flexible logging.

- **Validation:**
  - Incorporates FluentValidation for model validation.

- **Hashing:**
  - Provides hashing utilities for secure data hashing.

- **HTTP Client:**
  - Configures HttpClient for making HTTP requests.

- **OpenAPI Documentation:**
  - Configures Swagger and SwaggerUI for API documentation.

- **CORS Configuration:**
  - Allows easy setup of CORS policies.

- **Middleware:**
  - Includes custom middleware for exception handling and initialization.
  - Adds various security-related middlewares (e.g., no-cache headers, redirect validation).

- **SignalR:**
  - Configures SignalR with environment-specific settings.

- **Dependency Injection:**
  - Registers necessary services and middlewares for dependency injection.

- **Controller Configuration:**
  - Sets up controllers with Newtonsoft.Json for JSON serialization.

- **Security:**
  - Adds multiple security headers and policies.

## Usage

### Setting up your project

1. **Configure Services:**

   In your `Program.cs`, use the `ConfigureServices` method to configure all necessary services.

   ```csharp
   using Microsoft.Extensions.Configuration;
   using StoneKit.Infrastructure;

   var builder = WebApplication.CreateBuilder(args);

   var configOptions = new AppConfigurationOptions
   {
       // Set your options here
   };

   builder.ConfigureServices(configOptions);

   var app = builder.BuildApp(configOptions);

   app.Run();
   ```

2. **App Configuration Options:**

   Define your configuration options to customize the setup.

   ```csharp
   public class AppConfigurationOptions : BaseConfiguration
   {
       // Define properties like AppName, SerilogOptions, OpenApiOptions, etc.
   }
   ```

3. **FluentValidation:**

   Add validators to your project by inheriting from `AbstractValidator<T>` and ensure they are in the configured assembly.

   ```csharp
   using FluentValidation;

   public class SampleModelValidator : AbstractValidator<SampleModel>
   {
       public SampleModelValidator()
       {
           RuleFor(x => x.Property).NotEmpty();
       }
   }
   ```

### Example Middleware

```csharp
public class ExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }
}
```

### Adding Hashing Utility

```csharp
public interface IHasher
{
    string Hash(string input);
}

public class Hasher : IHasher
{
    public string Hash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}
```

### Configuring OpenAPI

```csharp
public static void ConfigureOpenApi(this WebApplicationBuilder builder, string appName, OpenApiOptions options, Assembly validatorAssembly)
{
    // OpenAPI configuration logic
}
```

## Contributions

Contributions and feedback are welcome! Feel free to submit issues, feature requests, or pull requests on the [GitHub repository](https://github.com/desmati/StoneKit/).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
