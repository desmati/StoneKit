using StoneKit.TransverseMapper;
using StoneKit.TransverseMapper.Binding;
using StoneKit.TransverseMapper.Mappers.Builder;
using StoneKit.TransverseMapper.Mappers.Classes.Members;

using System.Reflection;

namespace UnitTests
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

        public IDynamicAssembly Assembly => DynamicAssemblyBuilder.Get();

        public Func<string, string, bool> NameMatching => TargetMapperBuilder.DefaultNameMatching;

        public Maybe<BindingConfig> GetBindingConfig(TypePair typePair)
        {
            return _bindingConfig;
        }

        public MapperBuilder GetMapperBuilder(TypePair typePair)
        {
            throw new NotImplementedException();
        }

        public MapperBuilder GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember)
        {
            throw new NotImplementedException();
        }
    }
}
