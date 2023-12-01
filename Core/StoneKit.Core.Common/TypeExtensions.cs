using System.Collections;
using System.Reflection.Emit;

namespace System.Reflection
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Creates a Type from a TypeBuilder.
        /// </summary>
        public static Type CreateType(TypeBuilder typeBuilder)
        {
            return typeBuilder.CreateTypeInfo().AsType();
        }

        /// <summary>
        /// Gets the element type of a collection type.
        /// </summary>
        public static Type? GetCollectionItemType(this Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            if (type.IsGenericType && type.IsIEnumerableOf())
            {
                return type.GetGenericArguments().First();
            }

            return typeof(object);
        }

        /// <summary>
        /// Gets the default constructor of a type.
        /// </summary>
        public static ConstructorInfo? GetDefaultCtor(this Type type)
        {
            return type?.GetConstructor(Type.EmptyTypes);
        }

        /// <summary>
        /// Gets the key and value types of a dictionary type.
        /// </summary>
        public static KeyValuePair<Type, Type> GetDictionaryItemTypes(this Type type)
        {
            if (type.IsDictionaryOf())
            {
                Type[] types = type.GetGenericArguments();
                return new KeyValuePair<Type, Type>(types[0], types[1]);
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a generic method of a type.
        /// </summary>
        public static MethodInfo? GetGenericMethod(this Type type, string methodName, params Type[] arguments)
        {
            return type?.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)
                       ?.MakeGenericMethod(arguments);
        }

        /// <summary>
        /// Checks if a type has a default constructor.
        /// </summary>
        public static bool HasDefaultCtor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }

        /// <summary>
        /// Checks if a type is a dictionary.
        /// </summary>
        public static bool IsDictionaryOf(this Type type)
        {
            return type.IsGenericType &&
                   (type.GetGenericTypeDefinition() == typeof(Dictionary<,>) ||
                    type.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        /// <summary>
        /// Checks if a type implements IEnumerable.
        /// </summary>
        public static bool IsIEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// Checks if a type is IEnumerable of a specific type.
        /// </summary>
        public static bool IsIEnumerableOf(this Type type)
        {
            return type.GetInterfaces()
                       .Any(x => x.IsGenericType &&
                                 x.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                                 !x.IsGenericType && x == typeof(IEnumerable));
        }

        /// <summary>
        /// Checks if a type is a list.
        /// </summary>
        public static bool IsListOf(this Type type)
        {
            return type.IsGenericType &&
                (type.GetGenericTypeDefinition() == typeof(List<>) ||
                 type.GetGenericTypeDefinition() == typeof(IList<>) ||
                 type.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        /// <summary>
        /// Checks if a type is nullable.
        /// </summary>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}