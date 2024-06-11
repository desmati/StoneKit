namespace Trustee.Providers;

/// <summary>
/// Options for configuring the Azure Key Vault provider.
/// </summary>
public class AzureKeyVaultProviderOptions
{
    /// <summary>
    /// Gets or sets the URI of the Azure Key Vault.
    /// </summary>
    public required string VaultUri { get; set; }

    /// <summary>
    /// Gets or sets the interval for polling the Azure Key Vault for changes.
    /// </summary>
    public TimeSpan PollingInterval { get; set; } = TimeSpan.FromMinutes(5);
}