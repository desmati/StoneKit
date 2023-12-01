# Dynamic Assembly Builder

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

This provides a dynamic assembly builder for creating dynamic types in C#.

## Overview

### IDynamicAssembly Interface

The `IDynamicAssembly` interface defines a method for working with dynamic assemblies:

- `DefineType(string typeName, Type parentType)`: Defines a new type in the dynamic assembly.

## Usage

### Creating Dynamic Types

```csharp
// Get an instance of the dynamic assembly builder
IDynamicAssembly dynamicAssembly = new DynamicAssembly("AssemblyName");

// Define a new type in the dynamic assembly
TypeBuilder typeBuilder = dynamicAssembly.DefineType("MyDynamicType", typeof(object));

// ... define members, methods, etc. ...

```
