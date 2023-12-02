using StoneKit.TransverseMapper.Common.Cache;
using StoneKit.TransverseMapper.Mappers;
using StoneKit.TransverseMapper.Mappers.Classes;
using StoneKit.TransverseMapper.Mappers.Collections;
using StoneKit.TransverseMapper.Mappers.Members;
using StoneKit.TransverseMapper.Mappers.Types.Convertible;
using StoneKit.TransverseMapper.Mappers.Types.Custom;

using System.Reflection;
using System.Reflection.Mapping;

namespace StoneKit.TransverseMapper.MapperBuilder
{
    internal sealed class TargetMapperBuilder : IMapperBuilderConfig
    {
        /// <summary>
        /// The default name matching function that performs a case-sensitive ordinal string comparison.
        /// </summary>
        public static readonly Func<string, string, bool> DefaultNameMatching = (source, target) => string.Equals(source, target, StringComparison.Ordinal);

        private readonly Dictionary<TypePair, BindingConfig> _bindingConfigs = new Dictionary<TypePair, BindingConfig>();
        private readonly ClassMapperBuilder _classMapperBuilder;
        private readonly CollectionMapperBuilder _collectionMapperBuilder;
        private readonly ConvertibleTypeMapperBuilder _convertibleTypeMapperBuilder;
        private readonly CustomTypeMapperBuilder _customTypeMapperBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetMapperBuilder"/> class.
        /// </summary>
        /// <param name="assembly">The dynamic assembly to be used.</param>
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
        /// Gets or sets the name matching function used for auto bindings.
        /// </summary>
        public Func<string, string, bool> NameMatching { get; private set; }

        /// <summary>
        /// Gets the dynamic assembly used by the mapper builder.
        /// </summary>
        public IDynamicAssembly Assembly { get; }

        /// <summary>
        /// Gets the binding configuration for the specified type pair, if available.
        /// </summary>
        /// <param name="typePair">The source and target types.</param>
        /// <returns>The binding configuration wrapped in a Maybe monad.</returns>
        public Maybe<BindingConfig> GetBindingConfig(TypePair typePair)
        {
            return _bindingConfigs.GetValue(typePair);
        }

        /// <summary>
        /// Gets the appropriate mapper builder for the specified parent type pair and mapping member.
        /// </summary>
        public MapperBuilderBase GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember)
        {
            if (_customTypeMapperBuilder.IsSupported(parentTypePair, mappingMember))
            {
                return _customTypeMapperBuilder;
            }

            return GetTypeMapperBuilder(mappingMember.TypePair);
        }

        /// <summary>
        /// Gets the appropriate mapper builder for the specified type pair.
        /// </summary>
        public MapperBuilderBase GetMapperBuilder(TypePair typePair)
        {
            return GetTypeMapperBuilder(typePair);
        }

        /// <summary>
        /// Sets the custom name matching function.
        /// </summary>
        /// <param name="nameMatching">The custom name matching function.</param>
        public void SetNameMatching(Func<string, string, bool> nameMatching)
        {
            NameMatching = nameMatching ?? throw new ArgumentNullException(nameof(nameMatching));
        }

        /// <summary>
        /// Builds a mapper for the specified type pair and binding configuration.
        /// </summary>
        public MapperBase Build(TypePair typePair, BindingConfig bindingConfig)
        {
            _bindingConfigs[typePair] = bindingConfig;
            return Build(typePair);
        }

        /// <summary>
        /// Builds a mapper for the specified type pair.
        /// </summary>
        public MapperBase Build(TypePair typePair)
        {
            MapperBuilderBase mapperBuilder = GetTypeMapperBuilder(typePair);
            MapperBase mapper = mapperBuilder.Build(typePair);
            return mapper;
        }

        private MapperBuilderBase GetTypeMapperBuilder(TypePair typePair)
        {
            if (_convertibleTypeMapperBuilder.IsSupported(typePair))
            {
                return _convertibleTypeMapperBuilder;
            }

            if (_collectionMapperBuilder.IsSupported(typePair))
            {
                return _collectionMapperBuilder;
            }

            return _classMapperBuilder;
        }
    }
}
