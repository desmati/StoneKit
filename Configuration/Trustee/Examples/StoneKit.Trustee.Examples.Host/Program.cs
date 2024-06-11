using Trustee.Host;
using Trustee.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfigurationService()
    .AddFileSystemProvider(c =>
    {
        c.Path = "./";
        c.SearchPattern = "*.settings.json";
    });

var app = builder.Build();

app
    .UseRouting()
    .UseEndpoints(endpoints =>
{
    endpoints.MapConfigurationService();
});

app.Run();