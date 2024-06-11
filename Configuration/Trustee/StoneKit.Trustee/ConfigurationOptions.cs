namespace Trustee.Client;

/// <summary>
/// Represents options for configuring the behavior of a configuration source.
/// </summary>
public class ConfigurationOptions
{
    /// <summary>
    /// Gets or sets the name or path of the configuration file relative to the configuration provider path.
    /// </summary>
    public required string ConfigurationName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether loading the file is optional. Defaults to false.
    /// </summary>
    public bool Optional { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the source will be loaded if the underlying file changes. Defaults to false.
    /// </summary>
    public bool ReloadOnChange { get; set; }

    /// <summary>
    /// Gets or sets the type of <see cref="IConfigurationParser"/> used to parse the remote configuration file.
    /// </summary>
    public IConfigurationParser? Parser { get; set; }
}
