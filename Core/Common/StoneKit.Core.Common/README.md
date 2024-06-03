# StoneKit Common Utilities and Extension methods

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

## Installation

You can install by using NuGet:

	PM> Install-Package StoneKit.Core.Common

### Extension Methods:*
```
Enumerable:
int Count(this IEnumerable source)
bool IsNullOrEmpty<T>(this IEnumerable<T> value)
void Iter<T>(this IEnumerable<T> value, Action<T> action) // Apply the given function to each element of the collection
void IterSafe<T>(this IEnumerable<T> value, Action<T> action, Action<Exception> exceptionHandler = null!)


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

Dictionary:
TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> value, TKey key)
void TryAdd<TKey, TValue>(this Dictionary<TKey, TValue>? dictionary, Dictionary<TKey, TValue>? itemsToAdd)


```