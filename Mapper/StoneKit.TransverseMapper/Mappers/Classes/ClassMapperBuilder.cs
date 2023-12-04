using StoneKit.TransverseMapper.Common.Caches;
using StoneKit.TransverseMapper.Mappers.Builder;
using StoneKit.TransverseMapper.Mappers.Classes.Members;

using System.Reflection;
using System.Reflection.Emit;

namespace StoneKit.TransverseMapper.Mappers.Classes
{
    /// <summary>
    /// Class responsible for building and emitting class mappers.
    /// </summary>
    internal sealed class ClassMapperBuilder : MapperBuilder
    {
        private readonly MapperCache _mapperCache;
        private const string CreateTargetInstanceMethod = "CreateTargetInstance";
        private const string MapClassMethod = "MapClass";
        private readonly MappingMemberBuilder _mappingMemberBuilder;
        private readonly MemberMapper _memberMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassMapperBuilder"/> class.
        /// </summary>
        /// <param name="mapperCache">The mapper cache used for storing mappers.</param>
        /// <param name="config">The configuration for the mapper builder.</param>
        public ClassMapperBuilder(MapperCache mapperCache, IMapperBuilderConfig config) : base(config)
        {
            _mapperCache = mapperCache;
            _memberMapper = new MemberMapper(mapperCache, config);
            _mappingMemberBuilder = new MappingMemberBuilder(config);
        }

        /// <summary>
        /// Gets the scope name for the class mappers.
        /// </summary>
        protected override string ScopeName => "ClassMappers";

        /// <summary>
        /// Builds a class mapper for the specified type pair.
        /// </summary>
        /// <param name="typePair">The type pair for which to build the class mapper.</param>
        /// <returns>The built class mapper.</returns>
        protected override Mapper BuildCore(TypePair typePair)
        {
            Type parentType = typeof(ClassMapper<,>).MakeGenericType(typePair.Source, typePair.Target);
            TypeBuilder typeBuilder = _assembly.DefineType(GetMapperFullName(), parentType);
            EmitCreateTargetInstance(typePair.Target, typeBuilder);

            MapperCacheItem rootMapperCacheItem = _mapperCache.AddStub(typePair);
            Maybe<MapperCache> mappers = EmitMapClass(typePair, typeBuilder);

            var rootMapper = (Mapper)Activator.CreateInstance(typeBuilder.CreateType())!;

            UpdateMappers(mappers, rootMapperCacheItem.Id, rootMapper);

            _mapperCache.ReplaceStub(typePair, rootMapper);

            mappers.Perform(x => rootMapper.AddMappers(x.Mappers));

            return rootMapper;
        }

        /// <summary>
        /// Builds a class mapper for the specified parent type pair and mapping member.
        /// </summary>
        /// <param name="parentTypePair">The parent type pair.</param>
        /// <param name="mappingMember">The mapping member.</param>
        /// <returns>The built class mapper.</returns>
        protected override Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(mappingMember.TypePair);
        }

        /// <summary>
        /// Checks if the specified type pair is supported by the class mapper.
        /// </summary>
        /// <param name="typePair">The type pair to check for support.</param>
        /// <returns><c>true</c> if the type pair is supported; otherwise, <c>false</c>.</returns>
        protected override bool IsSupportedCore(TypePair typePair)
        {
            return true;
        }

        /// <summary>
        /// Emits IL code to create an instance of the target type.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <param name="typeBuilder">The type builder.</param>
        private static void EmitCreateTargetInstance(Type targetType, TypeBuilder typeBuilder)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(CreateTargetInstanceMethod, OverrideProtected, targetType, Type.EmptyTypes);
            var codeGenerator = new CodeGenerator(methodBuilder.GetILGenerator());

            IEmitterType result = targetType.IsValueType ? EmitValueType(targetType, codeGenerator) : EmitRefType(targetType);

            EmitReturn.Return(result, targetType).Emit(codeGenerator);
        }

        /// <summary>
        /// Emits IL code to handle reference type.
        /// </summary>
        /// <param name="type">The reference type.</param>
        /// <returns>The emitter for the reference type handling.</returns>
        private static IEmitterType EmitRefType(Type type)
        {
            return type.HasDefaultCtor() ? EmitNewObj.NewObj(type) : EmitNull.Load();
        }

        /// <summary>
        /// Emits IL code to handle value type.
        /// </summary>
        /// <param name="type">The value type.</param>
        /// <param name="codeGenerator">The code generator.</param>
        /// <returns>The emitter for the value type handling.</returns>
        private static IEmitterType EmitValueType(Type type, CodeGenerator codeGenerator)
        {
            LocalBuilder builder = codeGenerator.DeclareLocal(type);
            EmitLocalVariable.Declare(builder).Emit(codeGenerator);
            return EmitBox.Box(EmitLocal.Load(builder));
        }

        /// <summary>
        /// Emits IL code for the MapClass method.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <param name="typeBuilder">The type builder.</param>
        /// <returns>The Maybe containing the mapper cache.</returns>
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
        /// <param name="typePair">The type pair.</param>
        /// <returns>The description of member emitters.</returns>
        private MemberEmitterDescription EmitMappingMembers(TypePair typePair)
        {
            List<MappingMemberPath> members = _mappingMemberBuilder.Build(typePair);
            MemberEmitterDescription result = _memberMapper.Build(typePair, members);
            return result;
        }

        /// <summary>
        /// Updates mappers in the cache and in the root mapper.
        /// </summary>
        /// <param name="mappers">The Maybe containing the mapper cache.</param>
        /// <param name="rootMapperId">The ID of the root mapper.</param>
        /// <param name="rootMapper">The root mapper.</param>
        private static void UpdateMappers(Maybe<MapperCache> mappers, int rootMapperId, Mapper rootMapper)
        {
            if (mappers.HasValue)
            {
                var result = new List<Mapper>();
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

    }
}
