namespace System.Reflection.Mapping
{
    /// <summary>
    /// Attribute to specify custom binding information for property mapping.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class BindAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindAttribute"/> class.
        /// </summary>
        /// <param name="targetMemberName">The name of the target member to bind to.</param>
        /// <param name="targetType">The target type to which the attribute applies. If null, the attribute applies to all types.</param>
        public BindAttribute(string targetMemberName, Type targetType = null!)
        {
            MemberName = targetMemberName;
            TargetType = targetType;
        }

        /// <summary>
        /// Gets the target type to which the attribute applies. If null, the attribute applies to all types.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// Gets the name of the target member to bind to.
        /// </summary>
        public string MemberName { get; }
    }
}
