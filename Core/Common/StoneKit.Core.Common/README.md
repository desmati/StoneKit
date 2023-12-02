# StoneKit Common Utilities and Extension methods

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

## Installation

You can install by using NuGet:

```bash
nuget install StoneKit.Core.Common
```

### Extension Methods:*
```
bool IsNotNull(this object obj)
bool IsNull(this object obj)
Type CreateType(TypeBuilder typeBuilder)
Type? GetCollectionItemType(this Type type)
ConstructorInfo? GetDefaultCtor(this Type type)
KeyValuePair<Type, Type> GetDictionaryItemTypes(this Type type)
MethodInfo? GetGenericMethod(this Type type, string methodName, params Type[] arguments)
bool HasDefaultCtor(this Type type)
bool IsDictionaryOf(this Type type)
bool IsIEnumerable(this Type type)
bool IsIEnumerableOf(this Type type)
bool IsListOf(this Type type)
bool IsNullable(this Type type)

TValue? GetValue<TKey, TValue>(this IDictionary<TKey, TValue> value, TKey key)


```