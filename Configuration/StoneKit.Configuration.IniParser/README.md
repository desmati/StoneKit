# StoneKit.Configuration.IniParser

A simple and efficient INI file parser for .NET 8+

[![NuGet Version](https://img.shields.io/nuget/v/StoneKit.Configuration.IniParser.svg)](https://www.nuget.org/packages/StoneKit.Configuration.IniParser/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

## Overview

`StoneKit.Configuration.IniParser` is a lightweight library for parsing INI files in .NET 8+ applications. 
It allows you to read and manipulate configuration settings stored in INI files with ease.

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