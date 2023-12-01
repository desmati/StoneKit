# Reflection Extensions

This library provides a set of extension methods to simplify common tasks related to reflection in C#.

## MemberInfoExtensions

### GetAttribute\<TAttribute>

Gets a specified attribute from a member, if present.

```csharp
public static TAttribute? GetAttribute<TAttribute>(this MemberInfo value)
    where TAttribute : Attribute
```

Usage:

```csharp
MethodInfo methodInfo = // get MethodInfo
MyAttribute? attribute = methodInfo.GetAttribute<MyAttribute>();
```

### GetAttributes\<TAttribute>

Gets a list of specified attributes from a member.

```csharp
public static List<TAttribute> GetAttributes<TAttribute>(this MemberInfo value)
    where TAttribute : Attribute
```

Usage:

```csharp
PropertyInfo propertyInfo = // get PropertyInfo
List<MyAttribute> attributes = propertyInfo.GetAttributes<MyAttribute>();
```

### GetMemberType

Gets the type of the member.

```csharp
public static Type GetMemberType(this MemberInfo value)
```

Usage:

```csharp
FieldInfo fieldInfo = // get FieldInfo
Type fieldType = fieldInfo.GetMemberType();
```

### IsField, IsProperty, IsMethod

Determines whether the member is a field, property, or method.

```csharp
public static bool IsField(this MemberInfo value)
public static bool IsProperty(this MemberInfo value)
private static bool IsMethod(this MemberInfo value)
```

Usage:

```csharp
MethodInfo methodInfo = // get MethodInfo
bool isMethod = methodInfo.IsMethod();
```

## TypeExtensions

### IsValueType, IsPrimitive, IsEnum, IsGenericType

Determines whether the specified type is a value type, primitive type, enumeration type, or generic type.

```csharp
public static bool IsValueType(Type type)
public static bool IsPrimitive(Type type)
public static bool IsEnum(Type type)
public static bool IsGenericType(Type type)
```

Usage:

```csharp
Type myType = // get Type
bool isEnum = myType.IsEnum();
```

### CreateType

Creates a `System.Type` object based on the provided `TypeBuilder`.

```csharp
public static Type CreateType(TypeBuilder typeBuilder)
```

Usage:

```csharp
TypeBuilder typeBuilder = // create TypeBuilder
Type createdType = TypeExtensions.CreateType(typeBuilder);
```

### BaseType

Gets the base type of the specified type.

```csharp
public static Type? BaseType(Type type)
```

Usage:

```csharp
Type myType = // get Type
Type? baseType = TypeExtensions.BaseType(myType);
```
