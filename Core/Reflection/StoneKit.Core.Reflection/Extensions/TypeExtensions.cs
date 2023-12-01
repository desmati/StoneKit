using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// Provides extension methods for working with types.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified type is a value type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is a value type; otherwise, false.</returns>
        public static bool IsValueType(Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        /// <summary>
        /// Determines whether the specified type is a primitive type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is a primitive type; otherwise, false.</returns>
        public static bool IsPrimitive(Type type)
        {
            return type.GetTypeInfo().IsPrimitive;
        }

        /// <summary>
        /// Determines whether the specified type is an enumeration type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is an enumeration type; otherwise, false.</returns>
        public static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        /// <summary>
        /// Determines whether the specified type is a generic type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is a generic type; otherwise, false.</returns>
        public static bool IsGenericType(Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        /// <summary>
        /// Creates a <see cref="System.Type"/> object based on the provided <see cref="TypeBuilder"/>.
        /// </summary>
        /// <param name="typeBuilder">The TypeBuilder instance.</param>
        /// <returns>The created Type object.</returns>
        public static Type CreateType(TypeBuilder typeBuilder)
        {
            return typeBuilder.CreateTypeInfo().AsType();
        }

        /// <summary>
        /// Gets the base type of the specified type.
        /// </summary>
        /// <param name="type">The type to get the base type for.</param>
        /// <returns>The base type of the specified type.</returns>
        public static Type? BaseType(Type type)
        {
            return type.GetTypeInfo().BaseType;
        }
    }
}
