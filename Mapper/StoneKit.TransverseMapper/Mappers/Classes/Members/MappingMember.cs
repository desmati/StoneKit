using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Classes.Members
{
    /// <summary>
    /// Represents a mapping member, which includes source and target members along with their type pair.
    /// </summary>
    internal sealed class MappingMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingMember"/> class.
        /// </summary>
        /// <param name="source">The source member information.</param>
        /// <param name="target">The target member information.</param>
        /// <param name="typePair">The type pair associated with the mapping member.</param>
        public MappingMember(MemberInfo source, MemberInfo target, TypePair typePair)
        {
            Source = source;
            Target = target;
            TypePair = typePair;
        }

        /// <summary>
        /// Gets the source member information.
        /// </summary>
        public MemberInfo Source { get; }

        /// <summary>
        /// Gets the target member information.
        /// </summary>
        public MemberInfo Target { get; }

        /// <summary>
        /// Gets the type pair associated with the mapping member.
        /// </summary>
        public TypePair TypePair { get; }
    }
}
