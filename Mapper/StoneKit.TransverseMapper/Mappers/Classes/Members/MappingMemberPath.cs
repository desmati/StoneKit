using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Classes.Members
{
    /// <summary>
    /// Represents a path of mapping members, including source and target members, type pairs, and additional information about the path.
    /// </summary>
    internal sealed class MappingMemberPath
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingMemberPath"/> class for a path with multiple source and target members.
        /// </summary>
        /// <param name="source">The list of source members in the path.</param>
        /// <param name="target">The list of target members in the path.</param>
        public MappingMemberPath(List<MemberInfo> source, List<MemberInfo> target)
            : this(source, target, new TypePair(source[source.Count - 1].GetMemberType(), target[target.Count - 1].GetMemberType()))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingMemberPath"/> class for a path with a single source and target member.
        /// </summary>
        /// <param name="source">The source member in the path.</param>
        /// <param name="target">The target member in the path.</param>
        public MappingMemberPath(MemberInfo source, MemberInfo target)
            : this(new List<MemberInfo> { source }, new List<MemberInfo> { target }, new TypePair(source.GetMemberType(), target.GetMemberType()))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingMemberPath"/> class for a path with a single source and target member and a specified type pair.
        /// </summary>
        /// <param name="source">The source member in the path.</param>
        /// <param name="target">The target member in the path.</param>
        /// <param name="typePair">The type pair associated with the path.</param>
        public MappingMemberPath(MemberInfo source, MemberInfo target, TypePair typePair)
            : this(new List<MemberInfo> { source }, new List<MemberInfo> { target }, typePair)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingMemberPath"/> class for a path with multiple source and target members and a specified type pair.
        /// </summary>
        /// <param name="source">The list of source members in the path.</param>
        /// <param name="target">The list of target members in the path.</param>
        /// <param name="typePair">The type pair associated with the path.</param>
        public MappingMemberPath(List<MemberInfo> source, List<MemberInfo> target, TypePair typePair)
        {
            Source = source;
            OneLevelSource = source.Count == 1;
            OneLevelTarget = target.Count == 1;
            Target = target;
            TypePair = typePair;
            Tail = new MappingMember(source[source.Count - 1], target[target.Count - 1], typePair);
            Head = new MappingMember(source[0], target[0], new TypePair(source[0].GetMemberType(), target[0].GetMemberType()));
        }

        /// <summary>
        /// Gets a value indicating whether the source path has only one level (a single member).
        /// </summary>
        public bool OneLevelSource { get; }

        /// <summary>
        /// Gets a value indicating whether the target path has only one level (a single member).
        /// </summary>
        public bool OneLevelTarget { get; }

        /// <summary>
        /// Gets the list of source members in the path.
        /// </summary>
        public List<MemberInfo> Source { get; }

        /// <summary>
        /// Gets the list of target members in the path.
        /// </summary>
        public List<MemberInfo> Target { get; }

        /// <summary>
        /// Gets the type pair associated with the path.
        /// </summary>
        public TypePair TypePair { get; }

        /// <summary>
        /// Gets the tail member of the path.
        /// </summary>
        public MappingMember Tail { get; }

        /// <summary>
        /// Gets the head member of the path.
        /// </summary>
        public MappingMember Head { get; }
    }
}
