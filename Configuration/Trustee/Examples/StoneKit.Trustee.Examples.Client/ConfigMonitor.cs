using Microsoft.Extensions.Options;

namespace StoneKit.Trustee.Examples.Client;

public class ConfigMonitor
{
    private readonly IOptionsMonitor<TestConfig> _testConfig;

    public ConfigMonitor(IOptionsMonitor<TestConfig> testConfig)
    {
        _testConfig = testConfig;
    }

    public async Task Display(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var config = _testConfig.CurrentValue;
            Console.WriteLine(config.Text);

            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
    }
}
