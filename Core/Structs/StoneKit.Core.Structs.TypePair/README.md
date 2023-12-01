# TypePair Struct

The `TypePair` struct represents a pair of types, commonly used for object mapping and type conversion. It includes various properties and methods to facilitate type-related operations.


## Installation

You can install by using NuGet:

```bash
nuget install StoneKit.Core.Structs.TypePair
```


## Usage

### Creating TypePairs

#### Using Constructor

```csharp
var typePair = new TypePair(typeof(SourceType), typeof(TargetType));
```

#### Using Generic Method

```csharp
var typePair = TypePair.Create<SourceType, TargetType>();
```

### Checking Type Characteristics

```csharp
// Check if the types are both enums
bool isEnumTypes = typePair.IsEnumTypes;

// Check if the types are both enumerable
bool isEnumerableTypes = typePair.IsEnumerableTypes;

// Check if the source type is nullable while the target type is not
bool isNullableToNotNullable = typePair.IsNullableToNotNullable;

// Check if the types are deep cloneable
bool isDeepCloneable = typePair.IsDeepCloneable;
```

### Checking for Type Conversion

```csharp
// Check if a TypeConverter is available for type conversion
bool hasTypeConverter = typePair.HasTypeConverter();
```

### Equality Comparison

```csharp
// Check if two TypePairs are equal
bool areEqual = typePair1.Equals(typePair2);
```

## Examples

```csharp
// Example: Creating TypePairs
var typePair1 = new TypePair(typeof(int), typeof(string));
var typePair2 = TypePair.Create<double, decimal>();

// Example: Checking Type Characteristics
bool isEnumTypes = typePair1.IsEnumTypes;
bool isDeepCloneable = typePair2.IsDeepCloneable;

// Example: Checking for Type Conversion
bool hasTypeConverter = typePair1.HasTypeConverter();

// Example: Equality Comparison
bool areEqual = typePair1.Equals(typePair2);
```

## License

This project is licensed under the [MIT License](LICENSE.md).
