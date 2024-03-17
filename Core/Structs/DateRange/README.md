# StoneKit.Core.Structs.DateRange

DateRange is a lightweight C# struct that represents a range of dates, providing various functionalities such as checking for inclusion of a date within the range, calculating duration, checking for overlaps, and more.

## Installation

You can install StoneKit.Core.Structs.DateRange via NuGet Package Manager:

```
PM> Install-Package StoneKit.Core.Structs.DateRange
```

## Usage

```csharp
 // Create a date range from January 1, 2023 to December 31, 2023
 var range = new DateRange(new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));
 
 // Calculate duration in days
 double durationInDays = range.TotalDays();
 Console.WriteLine($"Duration in days: {durationInDays}");
 
 // Check if a specific date falls within the date range
 DateTime dateToCheck = new DateTime(2023, 6, 15);
 bool isIncluded = range.IncludesDate(dateToCheck);
 Console.WriteLine($"Is {dateToCheck} included in the date range? {isIncluded}");
```

## Features

- **DateRange Struct:** Represents a range of dates with optional start and end dates.
- **Extension Methods:** Provides a set of extension methods to enhance the functionality of the DateRange struct:
  - `Duration()`: Calculates the duration between two dates.
  - `OverlapsWith()`: Checks if two date ranges overlap.
  - `Intersection()`: Calculates the intersection of two date ranges.
  - `Union()`: Calculates the union of two date ranges.
  - `Shift()`: Shifts the date range by a specified duration.
  - `IsEmpty()`: Checks if the date range is empty (has no duration).
  - `IsInfinite()`: Checks if the date range is infinite (has no end date).
  - `TotalDays()`: Gets the duration of the date range in days.
  - `Hours()`: Gets the duration of the date range in hours.
  - `Minutes()`: Gets the duration of the date range in minutes.
  - `Seconds()`: Gets the duration of the date range in seconds.
  - `TotalMilliseconds()`: Gets the duration of the date range in milliseconds.

## Contributions

Contributions and feedback are welcome! Feel free to submit issues, feature requests, or pull requests on the [GitHub repository](https://github.com/desmati/StoneKit/).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
