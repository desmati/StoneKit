using CommandLine;

using StoneKit.Configuration.IniParser;

var options = LoadConfigFile(out var anyError);
if ((options == null || anyError) && args.Length > 0)
{
    options = LoadConfigCommandline(args, out anyError);
    if (anyError)
    {
        Console.WriteLine("Invalid command line options.", string.Join(" ", args));
        DisplayHelp();
        return;
    }
}

if (options == null)
{
    options = Options.Default();
}

// Handle help option
if (options.Help)
{
    DisplayHelp();
    return;
}

// Handle generateConfig option
if (options.GenerateConfig)
{
    var iniPath = GenerateConfigFile(new Options()
    {
        RemoveBinObj = true,
        GenerateConfig = false,
        Interactive = false,
        Help = false,
        RemoveDotVs = false,
    });

    Console.WriteLine($"Default configuration file generated: {Path.GetFileName(iniPath)}");

    return;
}

// Set the root directory to the current directory
string rootDirectory = Directory.GetCurrentDirectory();
Console.WriteLine($"Cleaning up project in: {rootDirectory}");

// Check for interactive mode or no options provided
var isInteractive = options.Interactive;
if (isInteractive)
{
    // Run interactively if requested or no options provided
    options = AskQuestions();
}

// Get a list of project files in the root directory and its subdirectories
var projectFiles = Directory.GetFiles(rootDirectory, "*.csproj", SearchOption.AllDirectories)
                            .Concat(new[] { Path.Combine(rootDirectory, "dummy.csproj") }) // Include root directory as a dummy project file
                            .ToList();

// Iterate through project files and perform cleanup
foreach (var projectFile in projectFiles)
{
    string projectDirectory = Path.GetDirectoryName(projectFile);
    string dotVsPath = Path.Combine(projectDirectory, ".vs");
    string binPath = Path.Combine(projectDirectory, "bin");
    string objPath = Path.Combine(projectDirectory, "obj");

    // Remove bin and obj folders if specified
    if (Directory.Exists(binPath) && options.RemoveBinObj)
    {
        RemoveDirectory(binPath);
    }

    if (Directory.Exists(objPath) && options.RemoveBinObj)
    {
        RemoveDirectory(objPath);
    }

    // Remove .vs folder if specified
    if (Directory.Exists(dotVsPath) && options.RemoveDotVs)
    {
        RemoveDirectory(dotVsPath);
    }

}

Console.WriteLine("\nCleanup completed successfully.");

if (isInteractive)
{
    Console.Write("\nSave the configuration? (y/n): ");
    if (Console.ReadKey().Key == ConsoleKey.Y)
    {
        GenerateConfigFile(options);
    }
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();

static void DisplayHelp()
{
    Console.WriteLine("-h or --help \t\t to display help information.");
    Console.WriteLine("-b or --remove-bin-obj \t to remove bin and obj folders.");
    Console.WriteLine("-v or --remove-dot-vs \t to remove .vs folders.");
    Console.WriteLine("-g or --generate-config \t to generate a default INI configuration file.");
    Console.WriteLine("-i or --interactive \t to run the tool interactively.");

    Console.WriteLine("\nPress any key to exit...");
    Console.ReadKey();
}

/// <summary>
/// Asks questions interactively and updates the Options object.
/// </summary>
static Options AskQuestions()
{
    var options = new Options();

    Console.WriteLine("\nNo command-line arguments provided. Starting interactive mode.");

    Console.Write("\nDelete bin and obj folders? (y/n): ");
    options.RemoveBinObj = Console.ReadKey().Key == ConsoleKey.Y;

    Console.Write("\nDelete .vs folders? (y/n): ");
    options.RemoveDotVs = Console.ReadKey().Key == ConsoleKey.Y;

    Console.WriteLine("\nCleanup configuration:");
    Console.WriteLine($"Remove bin and obj folders: {options.RemoveBinObj}");
    Console.WriteLine($"Remove .vs folders: {options.RemoveDotVs}");

    return options;
}

/// <summary>
/// Removes a directory and its contents recursively.
/// </summary>
static void RemoveDirectory(string directoryPath)
{
    try
    {
        Directory.Delete(directoryPath, true);
        Console.WriteLine($"Directory '{directoryPath}' and its contents deleted successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error deleting directory: {ex.Message}");
    }
}

/// <summary>
/// Generates a default INI configuration file.
/// </summary>
static string GenerateConfigFile(Options options)
{
    var ini = new IniFile();
    ini["CleanUp Options"] = options;

    var iniFilePath = $"{AppDomain.CurrentDomain.FriendlyName}.ini";
    ini.Save(iniFilePath);

    Console.WriteLine($"\nConfiguration written to: {iniFilePath}");

    return iniFilePath;
}

/// <summary>
/// Loads INI configuration file.
/// </summary>
static Options? LoadConfigFile(out bool anyError)
{
    anyError = true;
    try
    {
        var iniFilePath = $"{AppDomain.CurrentDomain.FriendlyName}.ini";
        if (!File.Exists(iniFilePath))
        {
            return null;
        }

        var ini = IniFile.Parse<Options>(iniFilePath);

        anyError = false;

        return ini["CleanUp Options"];
    }
    catch
    {
        return null;
    }
}

static Options? LoadConfigCommandline(string[] args, out bool anyError)
{
    Options? result = null;

    var parsedArgs = Parser.Default.ParseArguments<Options>(args);

    parsedArgs.WithParsed(parsed =>
    {
        result = parsed;
    });

    anyError = parsedArgs.Errors.Count() > 0;

    return result;
}