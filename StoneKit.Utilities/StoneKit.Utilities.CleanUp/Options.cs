using CommandLine;

/// <summary>
/// Class representing command-line options
/// </summary>
public class Options
{
    [Option('b', "remove-bin-obj", Required = false, HelpText = "Remove bin and obj folders.")]
    public bool RemoveBinObj { get; set; }

    [Option('v', "remove-dot-vs", Required = false, HelpText = "Remove .vs folders.")]
    public bool RemoveDotVs { get; set; }

    [Option('i', "interactive", Required = false, HelpText = "Run the tool interactively.")]
    public bool Interactive { get; set; }

    [Option('g', "generate-config", Required = false, HelpText = "Generate a default INI configuration file.")]
    public bool GenerateConfig { get; set; }

    [Option('h', "help", Required = false, HelpText = "Display help information.")]
    public bool Help { get; set; }

    internal static Options Default()
    {
        return new Options()
        {
            RemoveBinObj = true,
            GenerateConfig = false,
            Interactive = true,
            Help = false,
            RemoveDotVs = false,
        };
    }
}