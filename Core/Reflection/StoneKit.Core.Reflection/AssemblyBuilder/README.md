# Dynamic Assembly Builder

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

This provides a dynamic assembly builder for creating dynamic types in C#.

## Overview

### IDynamicAssembly Interface

The `IDynamicAssembly` interface defines methods for working with dynamic assemblies:

- `DefineType(string typeName, Type parentType)`: Defines a new type in the dynamic assembly.
- `Save()`: Saves the dynamic assembly.

### DynamicAssemblyBuilder Class

The `DynamicAssemblyBuilder` class represents a dynamic assembly builder used for creating dynamic types:

- `Get()`: Gets an instance of the dynamic assembly (`IDynamicAssembly`).
  
## Usage

### Creating Dynamic Types

```csharp
// Get an instance of the dynamic assembly builder
IDynamicAssembly dynamicAssembly = DynamicAssemblyBuilder.Get();

// Define a new type in the dynamic assembly
TypeBuilder typeBuilder = dynamicAssembly.DefineType("MyDynamicType", typeof(object));

// ... define members, methods, etc. ...

// Save the dynamic assembly
dynamicAssembly.Save();
```

## Contributing

If you encounter any issues or have suggestions for improvements, feel free to open an [issue](../../issues) or create a [pull request](../../pulls).
