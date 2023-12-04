using StoneKit.TransverseMapper.Binding;
using StoneKit.TransverseMapper.Common.Caches;
using StoneKit.TransverseMapper.Mappers.Classes;
using StoneKit.TransverseMapper.Mappers.Classes.Members;
using StoneKit.TransverseMapper.Mappers.Collections;
using StoneKit.TransverseMapper.Mappers.Types.Convertible;
using StoneKit.TransverseMapper.Mappers.Types.Custom;

using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Builder
{
    /// <summary>
    /// Builder for creating Target Mappers in the TransverseMapper framework.
    /// </summary>
    internal sealed class TargetMapperBuilder : IMapperBuilderConfig
    {
        /// <summary>
        /// Default name matching function that performs a case-sensitive string comparison.
        /// </summary>
        public static readonly Func<string, string, bool> DefaultNameMatching = (source, target) =>
            string.Equals(source, target, StringComparison.Ordinal);

        private readonly Dictionary<TypePair, BindingConfig> _bindingConfigs = new Dictionary<TypePair, BindingConfig>();
        private readonly ClassMapperBuilder _classMapperBuilder;
        private readonly CollectionMapperBuilder _collectionMapperBuilder;
        private readonly ConvertibleTypeMapperBuilder _convertibleTypeMapperBuilder;
        private readonly CustomTypeMapperBuilder _customTypeMapperBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetMapperBuilder"/> class.
        /// </summary>
        /// <param name="assembly">The dynamic assembly used for mapping.</param>
        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));

            var mapperCache = new MapperCache();
            _classMapperBuilder = new ClassMapperBuilder(mapperCache, this);
            _collectionMapperBuilder = new CollectionMapperBuilder(mapperCache, this);
            _convertibleTypeMapperBuilder = new ConvertibleTypeMapperBuilder(this);
            _customTypeMapperBuilder = new CustomTypeMapperBuilder(this);

            NameMatching = DefaultNameMatching;
        }

        /// <summary>
        /// Gets or sets the custom name matching function for auto bindings.
        /// </summary>
        public Func<string, string, bool> NameMatching { get; private set; }

        /// <summary>
        /// Gets the dynamic assembly used for mapping.
        /// </summary>
        public IDynamicAssembly Assembly { get; }

        /// <summary>
        /// Gets the binding configuration for the specified type pair.
        /// </summary>
        /// <param name="typePair">The pair of types for which to get the binding configuration.</param>
        /// <returns>The binding configuration if available; otherwise, None.</returns>
        public Maybe<BindingConfig> GetBindingConfig(TypePair typePair)
        {
            Maybe<BindingConfig> result = _bindingConfigs.GetValue(typePair);
            return result;
        }

        /// <summary>
        /// Gets the appropriate MapperBuilder for the specified parent type pair and mapping member.
        /// </summary>
        /// <param name="parentTypePair">The pair of types representing the parent.</param>
        /// <param name="mappingMember">The mapping member for which to get the MapperBuilder.</param>
        /// <returns>The MapperBuilder instance.</returns>
        public MapperBuilder GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember)
        {
            if (_customTypeMapperBuilder.IsSupported(parentTypePair, mappingMember))
            {
                return _customTypeMapperBuilder;
            }
            return GetTypeMapperBuilder(mappingMember.TypePair);
        }

        /// <summary>
        /// Gets the appropriate MapperBuilder for the specified type pair.
        /// </summary>
        /// <param name="typePair">The pair of types for which to get the MapperBuilder.</param>
        /// <returns>The MapperBuilder instance.</returns>
        public MapperBuilder GetMapperBuilder(TypePair typePair)
        {
            return GetTypeMapperBuilder(typePair);
        }

        /// <summary>
        /// Sets the custom name matching function for auto bindings.
        /// </summary>
        /// <param name="nameMatching">The custom name matching function.</param>
        public void SetNameMatching(Func<string, string, bool> nameMatching)
        {
            NameMatching = nameMatching;
        }

        /// <summary>
        /// Builds a mapper for the specified type pair with the provided binding configuration.
        /// </summary>
        /// <param name="typePair">The pair of types for which to build the mapper.</param>
        /// <param name="bindingConfig">The binding configuration to be applied.</param>
        /// <returns>The built mapper instance.</returns>
        public Mapper Build(TypePair typePair, BindingConfig bindingConfig)
        {
            _bindingConfigs[typePair] = bindingConfig;
            return Build(typePair);
        }

        /// <summary>
        /// Builds a mapper for the specified type pair.
        /// </summary>
        /// <param name="typePair">The pair of types for which to build the mapper.</param>
        /// <returns>The built mapper instance.</returns>
        public Mapper Build(TypePair typePair)
        {
            MapperBuilder mapperBuilder = GetTypeMapperBuilder(typePair);
            Mapper mapper = mapperBuilder.Build(typePair);
            return mapper;
        }

        private MapperBuilder GetTypeMapperBuilder(TypePair typePair)
        {
            if (_convertibleTypeMapperBuilder.IsSupported(typePair))
            {
                return _convertibleTypeMapperBuilder;
            }
            else if (_collectionMapperBuilder.IsSupported(typePair))
            {
                return _collectionMapperBuilder;
            }
            return _classMapperBuilder;
        }
    }
}
