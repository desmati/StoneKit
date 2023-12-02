using StoneKit.TransverseMapper.MapperBuilder;
using StoneKit.TransverseMapper.Mappers.Members;

using System.Reflection;
using System.Reflection.Mapping;

namespace StoneKit.TransverseMapper.Tests
{
    internal class MappingBuilderConfigStub : IMapperBuilderConfig
    {
        private readonly Maybe<BindingConfig> _bindingConfig = Maybe<BindingConfig>.Empty;

        public MappingBuilderConfigStub()
        {
        }

        public MappingBuilderConfigStub(BindingConfig bindingConfig)
        {
            _bindingConfig = bindingConfig.ToMaybe();
        }

        public IDynamicAssembly Assembly => Transverse.Assembly;

        public Func<string, string, bool> NameMatching => TargetMapperBuilder.DefaultNameMatching;

        public Maybe<BindingConfig> GetBindingConfig(TypePair typePair)
        {
            return _bindingConfig;
        }

        public MapperBuilderBase GetMapperBuilder(TypePair typePair)
        {
            throw new NotImplementedException();
        }

        public MapperBuilderBase GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember)
        {
            throw new NotImplementedException();
        }
    }
}
