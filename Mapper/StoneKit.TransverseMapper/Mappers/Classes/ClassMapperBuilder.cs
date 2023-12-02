using StoneKit.TransverseMapper.Common.Cache;
using StoneKit.TransverseMapper.MapperBuilder;
using StoneKit.TransverseMapper.Mappers.Members;

using System.Reflection;
using System.Reflection.Emit;

namespace StoneKit.TransverseMapper.Mappers.Classes
{
    /// <summary>
    /// Class responsible for building mappers for class types.
    /// </summary>
    internal sealed class ClassMapperBuilder : MapperBuilderBase
    {
        private readonly MapperCache _mapperCache;
        private const string CreateTargetInstanceMethod = "CreateTargetInstance";
        private const string MapClassMethod = "MapClass";
        private readonly MappingMemberBuilder _mappingMemberBuilder;
        private readonly MemberMapper _memberMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassMapperBuilder"/> class.
        /// </summary>
        /// <param name="mapperCache">The mapper cache instance.</param>
        /// <param name="config">The mapper builder configuration.</param>
        public ClassMapperBuilder(MapperCache mapperCache, IMapperBuilderConfig config) : base(config)
        {
            _mapperCache = mapperCache;
            _memberMapper = new MemberMapper(mapperCache, config);
            _mappingMemberBuilder = new MappingMemberBuilder(config);
        }

        /// <inheritdoc/>
        protected override string ScopeName => "ClassMappers";

        /// <inheritdoc/>
        /// <param name="typePair">The type pair to build the mapper for.</param>
        /// <returns>The built mapper for the specified type pair.</returns>
        protected override MapperBase BuildCore(TypePair typePair)
        {
            // Create a dynamic type for the class mapper and generate mapping code
            Type parentType = typeof(ClassMapper<,>).MakeGenericType(typePair.Source, typePair.Target);
            TypeBuilder typeBuilder = _assembly.DefineType(GetMapperFullName(), parentType);
            EmitCreateTargetInstance(typePair.Target, typeBuilder);

            MapperCacheItem rootMapperCacheItem = _mapperCache.AddStub(typePair);
            Maybe<MapperCache> mappers = EmitMapClass(typePair, typeBuilder);

            var rootMapper = (MapperBase)Activator.CreateInstance(typeBuilder.CreateType()!)!;

            UpdateMappers(mappers, rootMapperCacheItem.Id, rootMapper);

            _mapperCache.ReplaceStub(typePair, rootMapper);

            mappers.Perform(x => rootMapper.AddMappers(x.Mappers));

            return rootMapper;
        }

        /// <summary>
        /// Updates the mappers with the root mapper.
        /// </summary>
        /// <param name="mappers">The mappers to update.</param>
        /// <param name="rootMapperId">The ID of the root mapper.</param>
        /// <param name="rootMapper">The root mapper.</param>
        private static void UpdateMappers(Maybe<MapperCache> mappers, int rootMapperId, MapperBase rootMapper)
        {
            if (mappers.HasValue)
            {
                var result = new List<MapperBase>();
                foreach (var item in mappers.Value.MapperCacheItems)
                {
                    if (item.Id != rootMapperId)
                    {
                        result.Add(item.Mapper);
                    }
                    else
                    {
                        result.Add(null!);
                    }
                }
                result[rootMapperId] = rootMapper;
                rootMapper.AddMappers(result);
                foreach (var item in mappers.Value.MapperCacheItems)
                {
                    if (item.Id == rootMapperId)
                    {
                        continue;
                    }
                    item.Mapper?.UpdateRootMapper(rootMapperId, rootMapper);
                }
            }
        }

        /// <inheritdoc/>
        protected override MapperBase BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(mappingMember.TypePair);
        }

        /// <inheritdoc/>
        /// <param name="typePair">The type pair to check.</param>
        /// <returns>Always returns true for class mappers.</returns>
        protected override bool IsSupportedCore(TypePair typePair)
        {
            return true;
        }

        /// <summary>
        /// Emits IL code to create an instance of the target type.
        /// </summary>
        /// <param name="targetType">The target type to create an instance of.</param>
        /// <param name="typeBuilder">The type builder for the dynamic type.</param>
        private static void EmitCreateTargetInstance(Type targetType, TypeBuilder typeBuilder)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(CreateTargetInstanceMethod, OverrideProtected, targetType, Type.EmptyTypes);
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            IEmitterType result = targetType.IsValueType ? EmitValueType(targetType, codeGenerator) : EmitRefType(targetType);

            EmitReturn.Return(result, targetType).Emit(codeGenerator);
        }

        /// <summary>
        /// Emits IL code to handle reference types.
        /// </summary>
        /// <param name="type">The reference type.</param>
        /// <returns>The emitter type for the reference type.</returns>
        private static IEmitterType EmitRefType(Type type)
        {
            return type.HasDefaultCtor() ? EmitNewObj.NewObj(type) : EmitNull.Load();
        }

        /// <summary>
        /// Emits IL code to handle value types.
        /// </summary>
        /// <param name="type">The value type.</param>
        /// <param name="codeGenerator">The IL code generator.</param>
        /// <returns>The emitter type for the value type.</returns>
        private static IEmitterType EmitValueType(Type type, CodeGenerator codeGenerator)
        {
            LocalBuilder builder = codeGenerator.DeclareLocal(type);
            EmitLocalVariable.Declare(builder).Emit(codeGenerator);
            return EmitBox.Box(EmitLocal.Load(builder));
        }

        /// <summary>
        /// Emits IL code to map the class.
        /// </summary>
        /// <param name="typePair">The type pair to map.</param>
        /// <param name="typeBuilder">The type builder for the dynamic type.</param>
        /// <returns>The cached mappers for the mapping operation.</returns>
        private Maybe<MapperCache> EmitMapClass(TypePair typePair, TypeBuilder typeBuilder)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(MapClassMethod,
                OverrideProtected,
                typePair.Target,
                new[] { typePair.Source, typePair.Target });
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            var emitterComposite = new EmitComposite();

            MemberEmitterDescription emitterDescription = EmitMappingMembers(typePair);

            emitterComposite.Add(emitterDescription.Emitter);
            emitterComposite.Add(EmitReturn.Return(EmitArgument.Load(typePair.Target, 2)));
            emitterComposite.Emit(codeGenerator);
            return emitterDescription.MapperCache;
        }

        /// <summary>
        /// Emits IL code for mapping members.
        /// </summary>
        /// <param name="typePair">The type pair for which to emit mapping members.</param>
        /// <returns>The description of the member emitter.</returns>
        private MemberEmitterDescription EmitMappingMembers(TypePair typePair)
        {
            List<MappingMemberPath> members = _mappingMemberBuilder.Build(typePair);
            MemberEmitterDescription result = _memberMapper.Build(typePair, members);
            return result;
        }
    }
}
