namespace System.Reflection.Mapping
{
    /// <summary>
    /// Attribute to mark properties to be ignored during mapping.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class IgnoreAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreAttribute"/> class.
        /// </summary>
        /// <param name="targetType">The target type to which the attribute applies. If null, the attribute applies to all types.</param>
        public IgnoreAttribute(Type targetType = null!)
        {
            TargetType = targetType;
        }

        /// <summary>
        /// Gets the target type to which the attribute applies. If null, the attribute applies to all types.
        /// </summary>
        public Type TargetType { get; }
    }
}
