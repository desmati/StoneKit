using StoneKit.TransverseMapper.Common.Cache;
using StoneKit.TransverseMapper.MapperBuilder;

using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Members
{
    /// <summary>
    /// Responsible for building member mapping information.
    /// </summary>
    internal sealed class MemberMapper
    {
        private readonly IMapperBuilderConfig _config;
        private readonly MapperCache _mapperCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMapper"/> class.
        /// </summary>
        /// <param name="mapperCache">The mapper cache.</param>
        /// <param name="config">The configuration for the mapper builder.</param>
        public MemberMapper(MapperCache mapperCache, IMapperBuilderConfig config)
        {
            _mapperCache = mapperCache;
            _config = config;
        }

        /// <summary>
        /// Builds member emitter description based on the provided mapping information.
        /// </summary>
        /// <param name="parentTypePair">The source and target type pair of the parent mapping.</param>
        /// <param name="members">The mapping member paths.</param>
        /// <returns>The member emitter description.</returns>
        public MemberEmitterDescription Build(TypePair parentTypePair, List<MappingMemberPath> members)
        {
            var emitComposite = new EmitComposite();
            foreach (var path in members)
            {
                IEmitter emitter = Build(parentTypePair, path);
                emitComposite.Add(emitter);
            }
            var result = new MemberEmitterDescription(emitComposite, _mapperCache);
            result.AddMapper(_mapperCache);
            return result;
        }

        /// <summary>
        /// Builds an emitter based on the provided mapping member path.
        /// </summary>
        /// <param name="parentTypePair">The source and target type pair of the parent mapping.</param>
        /// <param name="memberPath">The mapping member path.</param>
        /// <returns>The emitter.</returns>
        private IEmitter Build(TypePair parentTypePair, MappingMemberPath memberPath)
        {
            if (memberPath.OneLevelTarget)
            {
                var sourceObject = EmitArgument.Load(memberPath.TypePair.Source, 1);
                var targetObject = EmitArgument.Load(memberPath.TypePair.Target, 2);

                var sourceMember = LoadMember(memberPath.Source, sourceObject, memberPath.Source.Count);
                var targetMember = LoadMember(memberPath.Target, targetObject, memberPath.Target.Count);

                IEmitterType convertedMember = ConvertMember(parentTypePair, memberPath.Tail, sourceMember, targetMember);

                IEmitter result = StoreTargetObjectMember(memberPath.Tail, targetObject, convertedMember);
                return result;
            }
            else
            {
                var targetObject = EmitArgument.Load(memberPath.Head.TypePair.Target, 2);
                var targetMember = LoadMember(memberPath.Target, targetObject, memberPath.Target.Count - 1);

                var sourceObject = EmitArgument.Load(memberPath.Head.TypePair.Source, 1);
                var sourceMember = LoadMember(memberPath.Source, sourceObject, memberPath.Source.Count);

                IEmitterType convertedMember = ConvertMember(parentTypePair, memberPath.Tail, sourceMember, targetMember);

                IEmitter result = StoreTargetObjectMember(memberPath.Tail, targetMember, convertedMember);
                return result;
            }
        }

        /// <summary>
        /// Converts the source member to the target member using mapping information.
        /// </summary>
        /// <param name="parentTypePair">The source and target type pair of the parent mapping.</param>
        /// <param name="member">The mapping member information.</param>
        /// <param name="sourceMember">The source member.</param>
        /// <param name="targetMember">The target member.</param>
        /// <returns>The converted member.</returns>
        private IEmitterType ConvertMember(TypePair parentTypePair, MappingMember member, IEmitterType sourceMember, IEmitterType targetMember)
        {
            if (member.TypePair.IsDeepCloneable)
            {
                return sourceMember;
            }

            MapperCacheItem mapperCacheItem = CreateMapperCacheItem(parentTypePair, member);

            IEmitterType result = mapperCacheItem.EmitMapMethod(sourceMember, targetMember);
            return result;
        }

        /// <summary>
        /// Creates a mapper cache item based on the mapping member.
        /// </summary>
        /// <param name="parentTypePair">The source and target type pair of the parent mapping.</param>
        /// <param name="mappingMember">The mapping member information.</param>
        /// <returns>The mapper cache item.</returns>
        private MapperCacheItem CreateMapperCacheItem(TypePair parentTypePair, MappingMember mappingMember)
        {
            var mapperCacheItemOption = _mapperCache.Get(mappingMember.TypePair);
            if (mapperCacheItemOption.HasValue)
            {
                return mapperCacheItemOption.Value;
            }

            MapperBuilderBase mapperBuilder = _config.GetMapperBuilder(parentTypePair, mappingMember);
            MapperBase mapper = mapperBuilder.Build(parentTypePair, mappingMember);
            MapperCacheItem mapperCacheItem = _mapperCache.Add(mappingMember.TypePair, mapper);
            return mapperCacheItem;
        }

        /// <summary>
        /// Loads a field from the source object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="field">The field to load.</param>
        /// <returns>The loaded field.</returns>
        private IEmitterType LoadField(IEmitterType source, FieldInfo field)
        {
            return EmitField.Load(source, field);
        }

        /// <summary>
        /// Loads a member based on the provided members, source object, and load level.
        /// </summary>
        /// <param name="members">The members to load.</param>
        /// <param name="sourceObject">The source object.</param>
        /// <param name="loadLevel">The load level.</param>
        /// <returns>The loaded member.</returns>
        private IEmitterType LoadMember(List<MemberInfo> members, IEmitterType sourceObject, int loadLevel)
        {
            IEmitterType dummySource = sourceObject;
            if (members.Count == 1)
            {
                return LoadMember(members[0], dummySource);
            }
            for (int i = 0; i < loadLevel; i++)
            {
                dummySource = LoadMember(members[i], dummySource);
            }
            return dummySource;
        }

        /// <summary>
        /// Loads a member from the source object.
        /// </summary>
        /// <param name="member">The member to load.</param>
        /// <param name="sourceObject">The source object.</param>
        /// <returns>The loaded member.</returns>
        private IEmitterType LoadMember(MemberInfo member, IEmitterType sourceObject)
        {
            IEmitterType result = null!;
            member.ToMaybe()
                  .Match(x => x.IsField(), x => result = LoadField(sourceObject, (FieldInfo)x))
                  .Match(x => x.IsProperty(), x => result = LoadProperty(sourceObject, (PropertyInfo)x));
            return result;
        }

        /// <summary>
        /// Loads a property from the source object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="property">The property to load.</param>
        /// <returns>The loaded property.</returns>
        private IEmitterType LoadProperty(IEmitterType source, PropertyInfo property)
        {
            return EmitProperty.Load(source, property);
        }

        /// <summary>
        /// Stores the converted member into the specified field of the target object.
        /// </summary>
        /// <param name="field">The field information.</param>
        /// <param name="targetObject">The target object.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The emitter type representing the stored field.</returns>
        private static IEmitterType StoreField(FieldInfo field, IEmitterType targetObject, IEmitterType value)
        {
            return EmitField.Store(field, targetObject, value);
        }

        /// <summary>
        /// Stores the converted member into the specified property of the target object.
        /// </summary>
        /// <param name="property">The property information.</param>
        /// <param name="targetObject">The target object.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The emitter type representing the stored property.</returns>
        private static IEmitterType StoreProperty(PropertyInfo property, IEmitterType targetObject, IEmitterType value)
        {
            return EmitProperty.Store(property, targetObject, value);
        }

        /// <summary>
        /// Stores the converted member into the target object member.
        /// </summary>
        /// <param name="member">The mapping member information.</param>
        /// <param name="targetObject">The target object.</param>
        /// <param name="convertedMember">The converted member to be stored.</param>
        /// <returns>The emitter type representing the stored member.</returns>
        private IEmitterType StoreTargetObjectMember(MappingMember member, IEmitterType targetObject, IEmitterType convertedMember)
        {
            IEmitterType result = null!;
            member.Target
                  .ToMaybe()
                  .Match(x => x.IsField(), x => result = StoreField((FieldInfo)x, targetObject, convertedMember))
                  .Match(x => x.IsProperty(), x => result = StoreProperty((PropertyInfo)x, targetObject, convertedMember));
            return result;
        }
    }
}
