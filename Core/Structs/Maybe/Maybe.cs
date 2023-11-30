namespace System
{
    /// <summary>
    /// Generic struct for representing optional values according to Option Monad design pattern.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <see cref="https://www.pluralsight.com/tech-blog/maybe"/>
    /// <see cref="https://en.wikipedia.org/wiki/Monad_(functional_programming)"/>
    public struct Maybe<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T}"/> struct with a value and an optional flag.
        /// </summary>
        /// <param name="value">The value of the Maybe monad.</param>
        /// <param name="hasValue">Optional flag indicating whether the Maybe monad has a value (default is true).</param>
        public Maybe(T value, bool hasValue = true)
        {
            HasValue = hasValue;
            Value = value;
        }

        /// <summary>
        /// Gets an empty Maybe monad.
        /// </summary>
        public static Maybe<T> Empty { get; } = new Maybe<T>(default!, false);

        /// <summary>
        /// Gets a value indicating whether the Maybe monad has no value.
        /// </summary>
        public bool HasNoValue => !HasValue;

        /// <summary>
        /// Gets a value indicating whether the Maybe monad has a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the value of the Maybe monad.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Conditionally executes an action based on a predicate.
        /// </summary>
        /// <param name="predicate">The condition to check against the value.</param>
        /// <param name="action">The action to execute if the predicate is true.</param>
        /// <returns>The current Maybe monad.</returns>
        public Maybe<T> Match(Func<T, bool> predicate, Action<T> action)
        {
            // If the Maybe monad has no value, return an empty Maybe monad
            if (HasNoValue)
            {
                return Empty;
            }

            // If the predicate is true, execute the action with the value
            if (predicate(Value))
            {
                action(Value);
            }

            // Return the current Maybe monad
            return this;
        }
    }
}
