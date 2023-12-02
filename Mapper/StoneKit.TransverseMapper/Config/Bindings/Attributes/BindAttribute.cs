namespace System.Reflection.Mapping
{
    /// <summary>
    /// Specifies the target member name and type for binding during object mapping.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class BindAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindAttribute"/> class.
        /// </summary>
        /// <param name="targetMemberName">The name of the target member for binding.</param>
        /// <param name="targetType">The target type for binding (optional).</param>
        public BindAttribute(string targetMemberName, Type targetType = null!)
        {
            MemberName = targetMemberName ?? throw new ArgumentNullException(nameof(targetMemberName));
            TargetType = targetType;
        }

        /// <summary>
        /// Gets the name of the target member for binding.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets the target type for binding.
        /// </summary>
        public Type TargetType { get; }
    }
}
