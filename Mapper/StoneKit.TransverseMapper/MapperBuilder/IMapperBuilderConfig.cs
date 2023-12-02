using StoneKit.TransverseMapper.Mappers.Members;

using System.Reflection;
using System.Reflection.Mapping;

namespace StoneKit.TransverseMapper.MapperBuilder
{
    /// <summary>
    /// Represents the configuration interface for a mapper builder.
    /// </summary>
    internal interface IMapperBuilderConfig
    {
        /// <summary>
        /// Gets the dynamic assembly used by the mapper builder.
        /// </summary>
        IDynamicAssembly Assembly { get; }

        /// <summary>
        /// Gets the name matching function used for auto bindings.
        /// </summary>
        Func<string, string, bool> NameMatching { get; }

        /// <summary>
        /// Gets the binding configuration for the specified type pair, if available.
        /// </summary>
        /// <param name="typePair">The source and target types.</param>
        /// <returns>The binding configuration wrapped in a Maybe monad.</returns>
        Maybe<BindingConfig> GetBindingConfig(TypePair typePair);

        /// <summary>
        /// Gets the appropriate mapper builder for the specified type pair.
        /// </summary>
        /// <param name="typePair">The source and target types.</param>
        /// <returns>The mapper builder for the specified type pair.</returns>
        MapperBuilderBase GetMapperBuilder(TypePair typePair);

        /// <summary>
        /// Gets the appropriate mapper builder for the specified parent type pair and mapping member.
        /// </summary>
        /// <param name="parentTypePair">The parent source and target types.</param>
        /// <param name="mappingMember">The mapping member.</param>
        /// <returns>The mapper builder for the specified parent type pair and mapping member.</returns>
        MapperBuilderBase GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember);
    }
}
