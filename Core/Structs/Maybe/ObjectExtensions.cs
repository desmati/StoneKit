namespace System
{
    /// <summary>
    /// Extension methods for objects, providing conversion and type-checking functionalities.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts an object to a Maybe monad.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="value">The object to convert.</param>
        /// <returns>A Maybe monad containing the object if it is not null for reference types or not default for value types; otherwise, an empty Maybe monad.</returns>
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            // Check if the type is a value type and the value is null
            if (!typeof(T).IsValueType && ReferenceEquals(value, null))
            {
                return Maybe<T>.Empty;
            }

            // Create a Maybe monad with the provided value
            return new Maybe<T>(value);
        }

        /// <summary>
        /// Converts an object to a Maybe monad of a specific type.
        /// </summary>
        /// <typeparam name="TResult">The desired type of the Maybe monad.</typeparam>
        /// <param name="obj">The object to convert.</param>
        /// <returns>A Maybe monad containing the object if it can be cast to the specified type; otherwise, an empty Maybe monad.</returns>
        public static Maybe<TResult> ToType<TResult>(this object obj)
        {
            // Check if the object can be cast to the specified type
            if (obj is TResult result)
            {
                return new Maybe<TResult>(result);
            }

            // Return an empty Maybe monad if the cast is not possible
            return Maybe<TResult>.Empty;
        }
    }
}
