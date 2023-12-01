namespace System
{
    /// <summary>
    /// Extension methods for the Maybe monad, providing additional operations and functionalities.
    /// </summary>
    public static class MaybeExtensions
    {
        /// <summary>
        /// Performs an action on the value if the Maybe monad has a value.
        /// </summary>
        public static Maybe<T> Perform<T>(this Maybe<T> value, Action<T> action)
        {
            if (value.HasValue)
            {
                action(value.Value);
            }

            return value;
        }

        /// <summary>
        /// Performs an action when the Maybe monad is empty.
        /// </summary>
        public static Maybe<T> PerformOnEmpty<T>(this Maybe<T> value, Action action)
        {
            if (value.HasNoValue)
            {
                action();
            }

            return value;
        }

        /// <summary>
        /// Performs an action on the value regardless of whether the Maybe monad has a value.
        /// </summary>
        public static Maybe<T> Finally<T>(this Maybe<T> value, Action<T> action)
        {
            action(value.Value);
            return value;
        }

        /// <summary>
        /// Returns the original Maybe monad if it has a value; otherwise, returns a new Maybe monad with the specified default value.
        /// </summary>
        public static Maybe<T> Or<T>(this Maybe<T> value, T orValue)
        {
            if (value.HasValue)
            {
                return value;
            }

            return new Maybe<T>(orValue);
        }

        /// <summary>
        /// Maps the Maybe monad from one type to another using a provided function.
        /// </summary>
        public static Maybe<TResult> Map<TInput, TResult>(this Maybe<TInput> value, Func<TInput, Maybe<TResult>> func)
        {
            if (value.HasNoValue)
            {
                return Maybe<TResult>.Empty;
            }

            return func(value.Value);
        }

        /// <summary>
        /// Maps the Maybe monad from one type to another using a provided function.
        /// </summary>
        public static Maybe<TResult> Map<TInput, TResult>(this Maybe<TInput> value, Func<TInput, TResult> func)
        {
            if (value.HasNoValue)
            {
                return Maybe<TResult>.Empty;
            }

            return func(value.Value).ToMaybe();
        }

        /// <summary>
        /// Maps the Maybe monad from one type to another based on a predicate using a provided function.
        /// </summary>
        public static Maybe<TResult> Map<TInput, TResult>(this Maybe<TInput> value, Func<TInput, bool> predicate, Func<TInput, TResult> func)
        {
            if (value.HasNoValue || !predicate(value.Value))
            {
                return Maybe<TResult>.Empty;
            }

            return func(value.Value).ToMaybe();
        }

        /// <summary>
        /// Maps the Maybe monad to another type when it is empty, using a provided function.
        /// </summary>
        public static Maybe<T> MapOnEmpty<T>(this Maybe<T> value, Func<T> func)
        {
            if (value.HasNoValue)
            {
                return func().ToMaybe();
            }

            return value;
        }

        /// <summary>
        /// Selects and projects the value of the Maybe monad using provided functions.
        /// </summary>
        public static Maybe<V> SelectMany<T, U, V>(this Maybe<T> value, Func<T, Maybe<U>> func, Func<T, U, V> selector)
        {
            return value.Map(x => func(x).Map(y => selector(x, y).ToMaybe()));
        }

        /// <summary>
        /// Filters the Maybe monad based on a predicate.
        /// </summary>
        public static Maybe<T> Where<T>(this Maybe<T> value, Func<T, bool> predicate)
        {
            if (value.HasNoValue || !predicate(value.Value))
            {
                return Maybe<T>.Empty;
            }

            return value;
        }

        public static IEnumerable<T> ToValue<T>(this IEnumerable<Maybe<T>> value)
        {
            return value.Where(x => x.HasValue)
                        .Select(x => x.Value);
        }
    }
}
