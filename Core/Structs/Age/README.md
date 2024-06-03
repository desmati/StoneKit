# Age Struct

Age Struct is a lightweight C# library that provides a struct representing age in years and days, along with various extension methods for calculating and determining age based on a given date.

## Installation

You can install Age Struct via NuGet Package Manager:

```
PM> Install-Package StoneKit.Core.Structs.Age
```

## Usage

### Age Struct

The `Age` struct represents an age in years and days, providing properties for the number of years and days, and some predefined age limits for infants, children, and adults.

```csharp
using System;

// Create an Age instance
var age = new Age(5, 200);

// Access years and days
Console.WriteLine($"Years: {age.Years}, Days: {age.Days}");

// Use predefined age limits
var minAdultAge = Age.MinAgeAdult;
var maxChildAge = Age.MaxAgeChild;
var maxInfantAge = Age.MaxAgeInfant;
```

### Extension Methods

The library provides a set of extension methods for `DateTime` to calculate and determine age.

```csharp
using System;

// Calculate age based on birth date
DateTime birthday = new DateTime(2015, 6, 15);
Age age = birthday.GetAge();
Console.WriteLine($"Age: {age.Years} years, {age.Days} days");

// Calculate age based on birth date and a specific date
DateTime referenceDate = new DateTime(2024, 6, 1);
Age ageOnReferenceDate = birthday.GetAge(referenceDate);
Console.WriteLine($"Age on {referenceDate.ToShortDateString()}: {ageOnReferenceDate.Years} years, {ageOnReferenceDate.Days} days");

// Determine if a person is an infant
bool isInfant = birthday.IsInfant();
Console.WriteLine($"Is infant: {isInfant}");

// Determine if a person is a child
bool isChild = birthday.IsChild();
Console.WriteLine($"Is child: {isChild}");

// Determine if a person is an adult
bool isAdult = birthday.IsAdult();
Console.WriteLine($"Is adult: {isAdult}");
```

## Features

- **Age Struct:** Represents an age in years and days with predefined limits for infants, children, and adults.
- **Extension Methods:** Provides a set of extension methods for `DateTime` to:
  - `GetAge()`: Calculate age based on birth date.
  - `IsInfant()`: Determine if a person is an infant.
  - `IsChild()`: Determine if a person is a child.
  - `IsAdult()`: Determine if a person is an adult.


## Contributions

Contributions and feedback are welcome! Feel free to submit issues, feature requests, or pull requests on the [GitHub repository](https://github.com/desmati/StoneKit/).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
