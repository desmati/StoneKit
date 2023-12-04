using StoneKit.TransverseMapper.Binding;
using StoneKit.TransverseMapper.Mappers.Classes.Members;

using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Builder
{
    /// <summary>
    /// Interface for configuring MapperBuilder settings.
    /// </summary>
    internal interface IMapperBuilderConfig
    {
        /// <summary>
        /// Gets the dynamic assembly used for mapping.
        /// </summary>
        IDynamicAssembly Assembly { get; }

        /// <summary>
        /// Gets the custom name matching function for auto bindings.
        /// </summary>
        Func<string, string, bool> NameMatching { get; }

        /// <summary>
        /// Gets the binding configuration for the specified type pair.
        /// </summary>
        /// <param name="typePair">The pair of types for which to get the binding configuration.</param>
        /// <returns>The binding configuration if available; otherwise, None.</returns>
        Maybe<BindingConfig> GetBindingConfig(TypePair typePair);

        /// <summary>
        /// Gets the MapperBuilder for the specified type pair.
        /// </summary>
        /// <param name="typePair">The pair of types for which to get the MapperBuilder.</param>
        /// <returns>The MapperBuilder instance.</returns>
        MapperBuilder GetMapperBuilder(TypePair typePair);

        /// <summary>
        /// Gets the MapperBuilder for the specified parent type pair and mapping member.
        /// </summary>
        /// <param name="parentTypePair">The pair of types representing the parent.</param>
        /// <param name="mappingMember">The mapping member for which to get the MapperBuilder.</param>
        /// <returns>The MapperBuilder instance.</returns>
        MapperBuilder GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember);
    }
}
