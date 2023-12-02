namespace System.Reflection
{
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Gets the specified attribute from the member, if present.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve.</typeparam>
        /// <param name="value">The member information.</param>
        /// <returns>The attribute instance or null if not found.</returns>
        public static TAttribute? GetAttribute<TAttribute>(this MemberInfo value)
            where TAttribute : Attribute
        {
            return (TAttribute?)value
                ?.GetCustomAttributes(typeof(TAttribute), true)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets a list of specified attributes from the member.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve.</typeparam>
        /// <param name="value">The member information.</param>
        /// <returns>A list of attribute instances.</returns>
        public static List<TAttribute> GetAttributes<TAttribute>(this MemberInfo value)
            where TAttribute : Attribute
        {
            return value.GetCustomAttributes(typeof(TAttribute), true)
                        .OfType<TAttribute>()
                        .ToList();
        }

        /// <summary>
        /// Gets the type of the member.
        /// </summary>
        /// <param name="value">The member information.</param>
        /// <returns>The type of the member.</returns>
        public static Type GetMemberType(this MemberInfo value)
        {
            if (value.IsField())
            {
                return ((FieldInfo)value).FieldType;
            }
            if (value.IsProperty())
            {
                return ((PropertyInfo)value).PropertyType;
            }
            if (value.IsMethod())
            {
                return ((MethodInfo)value).ReturnType;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Determines whether the member is a field.
        /// </summary>
        /// <param name="value">The member information.</param>
        /// <returns>True if the member is a field; otherwise, false.</returns>
        public static bool IsField(this MemberInfo value)
        {
            return value?.MemberType == MemberTypes.Field;
        }

        /// <summary>
        /// Determines whether the member is a property.
        /// </summary>
        /// <param name="value">The member information.</param>
        /// <returns>True if the member is a property; otherwise, false.</returns>
        public static bool IsProperty(this MemberInfo value)
        {
            return value?.MemberType == MemberTypes.Property;
        }

        /// <summary>
        /// Determines whether the member is a method.
        /// </summary>
        /// <param name="value">The member information.</param>
        /// <returns>True if the member is a method; otherwise, false.</returns>
        private static bool IsMethod(this MemberInfo value)
        {
            return value?.MemberType == MemberTypes.Method;
        }
    }
}
