namespace System.Reflection.Mapping
{
    /// <summary>
    /// Specifies that the property should be ignored during object binding.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class IgnoreAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreAttribute"/> class.
        /// </summary>
        /// <param name="targetType">The target type to which the ignore attribute applies (optional).</param>
        public IgnoreAttribute(Type targetType = null!)
        {
            TargetType = targetType;
        }

        /// <summary>
        /// Gets the target type to which the ignore attribute applies.
        /// </summary>
        public Type TargetType { get; }
    }
}
