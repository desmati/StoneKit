# StoneKit.Configuration.IniParser

`StoneKit.Configuration.IniParser` not only simplifies the parsing of INI files but also offers additional features, including the ability to read and write objects. This library is designed to make working with configuration data in INI files more versatile and convenient.

[![NuGet Version](https://img.shields.io/nuget/v/StoneKit.Configuration.IniParser.svg)](https://www.nuget.org/packages/StoneKit.Configuration.IniParser/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

## Overview

In addition to basic INI file parsing, `StoneKit.Configuration.IniParser` provides advanced features for handling objects. This includes reading objects from INI files and writing object properties back to INI sections. The library automatically manages the serialization and deserialization of object properties, simplifying the configuration process.

## Installation

Install the library via NuGet Package Manager Console:

```powershell
Install-Package StoneKit.Configuration.IniParser
```

## Getting Started

### Loading INI File

```csharp
// Create an instance of IniFile and load data from the specified file
var iniFile = new IniFile();
iniFile.LoadFile("path/to/your/file.ini");

// Alternatively, use the static method to quickly load information from an INI file
var quickIniFile = IniFile.Parse("path/to/your/file.ini");
```

### Accessing Configuration Values

```csharp
// Retrieve a value from a specific section and key
string? value = iniFile["SectionName", "KeyName"];

// Retrieve a value with a default if not found
string defaultValue = iniFile["SectionName", "KeyName", "Default"];

// Get an array of section names
string[] sections = iniFile.Sections;

// Get an array of key names in a specific section
string[] keysInSection = iniFile.GetKeys("SectionName");
```

### Reading and Writing Objects

```csharp
// Define a sample configuration class
public class AppConfig
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string DatabaseName { get; set; }
    // ... other properties
}

// Load an instance of AppConfig from the INI file
AppConfig config = iniFile.Load<AppConfig>("Database");

// Modify the object
config.Server = "newServer";
config.Port = 8080;

// Save the modified object back to the INI file
iniFile.Save(config);
```

### Example INI File

```ini
; Example INI File

[Database]
Server=localhost
Port=5432
DatabaseName=mydatabase
Username=myuser
Password=mypassword

[AppSettings]
LogLevel=Info
MaxConnections=100
```

## Contributions

Contributions, issues, and feature requests are welcome. 
Feel free to submit a pull request or open an issue on 
[GitHub](https://github.com/desmati/StoneKit).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.