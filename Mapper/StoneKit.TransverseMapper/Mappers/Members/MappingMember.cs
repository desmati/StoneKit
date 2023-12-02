using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Members
{
    /// <summary>
    /// Represents a member mapping between source and target objects in the Transverse framework.
    /// </summary>
    internal sealed class MappingMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingMember"/> class.
        /// </summary>
        /// <param name="source">The member information of the source object.</param>
        /// <param name="target">The member information of the target object.</param>
        /// <param name="typePair">The type pair representing the source and target types.</param>
        public MappingMember(MemberInfo source, MemberInfo target, TypePair typePair)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            TypePair = typePair;
        }

        /// <summary>
        /// Gets the member information of the source object.
        /// </summary>
        public MemberInfo Source { get; }

        /// <summary>
        /// Gets the member information of the target object.
        /// </summary>
        public MemberInfo Target { get; }

        /// <summary>
        /// Gets the type pair representing the source and target types.
        /// </summary>
        public TypePair TypePair { get; }
    }
}
