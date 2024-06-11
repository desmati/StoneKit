using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using StoneKit.Trustee.Examples.Client;

using Trustee.Client;

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

IConfiguration localConfiguration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var configuration = new ConfigurationBuilder()
    .AddConfiguration(localConfiguration)
    .AddRemoteConfiguration(o =>
    {
        o.ServiceUri = "http://localhost:60080/configuration/";
        o.AddConfiguration(c =>
        {
            c.ConfigurationName = "test.settings.json";
            c.ReloadOnChange = true;
            c.Optional = false;
        });
    })
    .Build();

var services = new ServiceCollection();
services.AddConfigurationServices();
services.AddSingleton<ConfigMonitor>();
services.Configure<TestConfig>(configuration.GetSection("Config"));

var serviceProvider = services.BuildServiceProvider();

var configWriter = serviceProvider.GetService<ConfigMonitor>();

await configWriter!.Display();
