using StoneKit.TransverseMapper.MapperBuilder;
using StoneKit.TransverseMapper.Mappers.Members;

using System.Reflection.Mapping;

namespace StoneKit.TransverseMapper.Mappers.Types.Custom
{
    /// <summary>
    /// Builder for creating <see cref="CustomTypeMapper"/> instances based on custom type converters.
    /// </summary>
    internal sealed class CustomTypeMapperBuilder : MapperBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypeMapperBuilder"/> class.
        /// </summary>
        /// <param name="config">The mapper builder configuration.</param>
        public CustomTypeMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        /// <inheritdoc/>
        protected override string ScopeName => "CustomTypeMapper";

        /// <summary>
        /// Checks if the specified mapping member has a custom type converter.
        /// </summary>
        /// <param name="parentTypePair">The parent type pair.</param>
        /// <param name="mappingMember">The mapping member.</param>
        /// <returns>True if the mapping member has a custom type converter; otherwise, false.</returns>
        public bool IsSupported(TypePair parentTypePair, MappingMember mappingMember)
        {
            Maybe<BindingConfig> bindingConfig = _config.GetBindingConfig(parentTypePair);
            if (bindingConfig.HasNoValue)
            {
                return false;
            }
            return bindingConfig.Value.HasCustomTypeConverter(mappingMember.Target.Name);
        }

        /// <inheritdoc/>
        protected override MapperBase BuildCore(TypePair typePair)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        protected override MapperBase BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            Maybe<BindingConfig> bindingConfig = _config.GetBindingConfig(parentTypePair);
            Func<object, object> converter = bindingConfig.Value.GetCustomTypeConverter(mappingMember.Target.Name).Value;
            return new CustomTypeMapper(converter);
        }

        /// <inheritdoc/>
        protected override bool IsSupportedCore(TypePair typePair)
        {
            throw new NotSupportedException();
        }
    }
}
