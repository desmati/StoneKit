[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

# StoneKit - DotNet 8 Toolset

StoneKit is a comprehensive dotnet toolset consisting of various libraries and utilities to enhance and simplify common tasks in .NET development. Below are the key components of StoneKit:


## 1. Transverse - Quick Tiny Object Mapper

	PM> Install-Package StoneKit.TransverseMapper

[Usage and More Details](Mapper/StoneKit.TransverseMapper/README.md)

A tiny and quick object mapper for .Net v8.0. Inspired by AutoMapper and TinyMapper, Transverse aims to be the tiniest and fastest mapping library in dotnet.

Right now it performs faster than AutoMapper and I'm trying to improve it constantly. 

| Method                           | Mean      | Error      | StdDev    | Median    |
|----------------------------------|-----------|------------|-----------|-----------|
| CollectionMapping_AutoMapper     | 2.061 �s  | 0.0405 �s  | 0.0433 �s | 2.055 �s  |
| CollectionMapping_Transverse     | 1.802 �s  | 0.0649 �s  | 0.1872 �s | 1.722 �s  |
| CollectionMapping_Handwritten    | 1.474 �s  | 0.0379 �s  | 0.1075 �s | 1.459 �s  |

## 2. StoneKit Advanced Reflection Tools
This comprehensive library provides advanced tools for interacting with IL and leveraging reflection as part of StoneKit toolset.

	PM> Install-Package StoneKit.Core.Reflection

- [IL Code Generator](Core/Reflection/StoneKit.Core.Reflection/CodeGenerator/README.md)
- [Dynamic Assembly Builder](Core/Reflection/StoneKit.Core.Reflection/DynamicAssemblyBuilder/README.md)
- [Extension Methods](Core/Reflection/StoneKit.Core.Reflection/Extensions/README.md)

## 3. StoneKit Common Utilities and Extension methods
Handy extension methods that are optimize for dotnet 8

	PM> Install-Package StoneKit.Core.Common

[Usage and More Details](Core/Common/StoneKit.Core.Common/README.md)

## 4. Maybe Monad for C#

	PM> Install-Package StoneKit.Core.Strcuts.Maybe

[Usage and More Details](Core/Structs/Maybe/README.md)

The Maybe monad is a generic struct designed to represent optional values in C# following the Option Monad design pattern. It allows you to work with values that might be absent, helping to write more expressive and concise code. 

This library includes extension methods for additional operations and functionalities which makes it unique by keeping the struct size as small as possible. 

It has small memory footprints and performs much faster that other available implementations.

For a comprehensive understanding of the Maybe Monad, you can refer to the [Pluralsight tech blog](https://www.pluralsight.com/tech-blog/maybe) and [Wikipedia on Monad (functional programming)](https://en.wikipedia.org/wiki/Monad_(functional_programming)).

## 5. TypePair Struct

	PM> Install-Package StoneKit.Core.Structs.TypePair

[Usage and More Details](Core/Structs/StoneKit.Core.Structs.TypePair/README.md)

This `TypePair` struct represents a pair of types, commonly used for object mapping and type conversion.
