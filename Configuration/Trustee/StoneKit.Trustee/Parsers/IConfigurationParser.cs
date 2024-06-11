namespace Trustee.Client;

/// <summary>
/// Represents a parser for configuration files.
/// </summary>
public interface IConfigurationParser
{
    /// <summary>
    /// Parses the configuration data from the provided input stream.
    /// </summary>
    /// <param name="input">The input stream containing the configuration data.</param>
    /// <returns>A dictionary representing the parsed configuration data.</returns>
    IDictionary<string, string> Parse(Stream input);
}
