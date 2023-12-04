using StoneKit.TransverseMapper.Binding;
using StoneKit.TransverseMapper.Mappers.Builder;
using StoneKit.TransverseMapper.Mappers.Classes.Members;

namespace StoneKit.TransverseMapper.Mappers.Types.Custom
{
    /// <summary>
    /// Builder for creating a custom type mapper based on provided configurations.
    /// </summary>
    internal sealed class CustomTypeMapperBuilder : MapperBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypeMapperBuilder"/> class.
        /// </summary>
        /// <param name="config">The mapper builder configuration.</param>
        public CustomTypeMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        /// <summary>
        /// Gets the scope name for the custom type mapper builder.
        /// </summary>
        protected override string ScopeName => "CustomTypeMapper";

        /// <summary>
        /// Checks whether the given <paramref name="mappingMember"/> is supported for custom type mapping.
        /// </summary>
        /// <param name="parentTypePair">The parent type pair.</param>
        /// <param name="mappingMember">The mapping member.</param>
        /// <returns>True if the mapping member is supported; otherwise, false.</returns>
        public bool IsSupported(TypePair parentTypePair, MappingMember mappingMember)
        {
            Maybe<BindingConfig> bindingConfig = _config.GetBindingConfig(parentTypePair);
            if (bindingConfig.HasNoValue)
            {
                return false;
            }
            return bindingConfig.Value.HasCustomTypeConverter(mappingMember.Target.Name);
        }

        /// <summary>
        /// Builds a custom type mapper for the specified <paramref name="typePair"/>.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>The custom type mapper instance.</returns>
        protected override Mapper BuildCore(TypePair typePair)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Builds a custom type mapper for the specified <paramref name="parentTypePair"/> and <paramref name="mappingMember"/>.
        /// </summary>
        /// <param name="parentTypePair">The parent type pair.</param>
        /// <param name="mappingMember">The mapping member.</param>
        /// <returns>The custom type mapper instance.</returns>
        protected override Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            Maybe<BindingConfig> bindingConfig = _config.GetBindingConfig(parentTypePair);
            Func<object, object> converter = bindingConfig.Value.GetCustomTypeConverter(mappingMember.Target.Name).Value;
            return new CustomTypeMapper(converter);
        }

        /// <summary>
        /// Checks whether the specified type pair is supported for custom type mapping.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>True if the type pair is supported; otherwise, false.</returns>
        protected override bool IsSupportedCore(TypePair typePair)
        {
            throw new NotSupportedException();
        }
    }
}
