# Maybe Monad for C#

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

## Overview

The Maybe monad is a generic struct designed to represent optional values in C# following the Option Monad design pattern. It allows you to work with values that might be absent, helping to write more expressive and concise code. 

This library includes extension methods for additional operations and functionalities which makes it unique by keeping the struct size as small as possible. 

It has small memory footprints and performs much faster that other available implementations.

For a comprehensive understanding of the Maybe Monad, you can refer to the [Pluralsight tech blog](https://www.pluralsight.com/tech-blog/maybe) and [Wikipedia on Monad (functional programming)](https://en.wikipedia.org/wiki/Monad_(functional_programming)).


## Installation

You can install by using NuGet:

    PM> Install-Package StoneKit.Core.Strcuts.Maybe

## Usage

```csharp
var maybeWithValue = new Maybe<int>(42);
var maybeEmpty = Maybe<int>.Empty;
var toMaybe = 10.ToMaybe();

var maybe = Maybe<int>.Empty
    .Or(10)
    .Where(x => x > 5)
    .Perform(x=> Console.WriteLine($"{x} is greater than 5."))
    .Match(
        value => value < 50,
        value => Console.WriteLine($"{value} is smaller than 50.")
    )
    .Where(x=> x >100)
    .PerformOnEmpty(() => Console.WriteLine("Performing action on empty Maybe"))
    .Or(100)
    .Finally(x=> Console.WriteLine("I'm done!"));
```

## Extensions

### `Perform`

Performs an action on the value if the Maybe monad has a value.

### `PerformOnEmpty`

Performs an action when the Maybe monad is empty.

### `Finally`

Performs an action on the value regardless of whether the Maybe monad has a value.

### `Or`

Returns the original Maybe monad if it has a value; otherwise, returns a new Maybe monad with the specified default value.

### `Map`

Maps the Maybe monad from one type to another using a provided function.

### `MapOnEmpty`

Maps the Maybe monad to another type when it is empty, using a provided function.

### `SelectMany`

Selects and projects the value of the Maybe monad using provided functions.

### `Where`

Filters the Maybe monad based on a predicate.

### `ToMaybe`

Converts an object to a Maybe monad.

### `ToType`

Converts an object to a Maybe monad of a specific type.

## Contributing

If you find any issues or have suggestions for improvements, feel free to open an [issue](../../issues) or create a [pull request](../../pulls).

## License

This Maybe Monad library is licensed under the [MIT License](LICENSE).
