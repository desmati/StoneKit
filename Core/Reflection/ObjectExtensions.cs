using System.Reflection.Emit;

namespace System.Reflection
{
    public static class ObjectExtensions
    {
        public static bool IsValueType(Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        public static bool IsPrimitive(Type type)
        {
            return type.GetTypeInfo().IsPrimitive;
        }

        public static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static bool IsGenericType(Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static Type CreateType(TypeBuilder typeBuilder)
        {
            return typeBuilder.CreateTypeInfo().AsType();
        }

        public static Type? BaseType(Type type)
        {
            return type.GetTypeInfo().BaseType;
        }
    }
}
