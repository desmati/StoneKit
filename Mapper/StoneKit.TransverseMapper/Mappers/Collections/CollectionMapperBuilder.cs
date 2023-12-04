using StoneKit.TransverseMapper.Common.Caches;
using StoneKit.TransverseMapper.Mappers.Builder;
using StoneKit.TransverseMapper.Mappers.Classes.Members;

using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace StoneKit.TransverseMapper.Mappers.Collections
{
    /// <summary>
    /// Builds mappers for mapping collections from source to target.
    /// </summary>
    internal sealed class CollectionMapperBuilder : MapperBuilder
    {
        private readonly MapperCache _mapperCache;
        private const string ConvertItemKeyMethod = "ConvertItemKey";
        private const string ConvertItemMethod = "ConvertItem";
        private const string DictionaryToDictionaryMethod = "DictionaryToDictionary";
        private const string DictionaryToDictionaryTemplateMethod = "DictionaryToDictionaryTemplate";
        private const string EnumerableToArrayMethod = "EnumerableToArray";
        private const string EnumerableToArrayTemplateMethod = "EnumerableToArrayTemplate";
        private const string EnumerableToListMethod = "EnumerableToList";
        private const string EnumerableToListTemplateMethod = "EnumerableToListTemplate";
        private const string EnumerableOfDeepCloneableToListTemplateMethod = "EnumerableOfDeepCloneableToListTemplate";

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionMapperBuilder"/> class.
        /// </summary>
        /// <param name="mapperCache">The mapper cache.</param>
        /// <param name="config">The mapper builder configuration.</param>
        public CollectionMapperBuilder(MapperCache mapperCache, IMapperBuilderConfig config) : base(config)
        {
            _mapperCache = mapperCache;
        }

        /// <summary>
        /// Gets the scope name for the collection mappers.
        /// </summary>
        protected override string ScopeName => "CollectionMappers";

        /// <summary>
        /// Builds a mapper for the specified type pair.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>The built mapper.</returns>
        protected override Mapper BuildCore(TypePair typePair)
        {
            Type parentType = typeof(CollectionMapper<,>).MakeGenericType(typePair.Source, typePair.Target);
            TypeBuilder typeBuilder = _assembly.DefineType(GetMapperFullName(), parentType);

            _mapperCache.AddStub(typePair);

            if (IsIEnumerableToList(typePair))
            {
                EmitEnumerableToList(parentType, typeBuilder, typePair);
            }
            else if (IsIEnumerableToArray(typePair))
            {
                EmitEnumerableToArray(parentType, typeBuilder, typePair);
            }
            else if (IsDictionaryToDictionary(typePair))
            {
                EmitDictionaryToDictionary(parentType, typeBuilder, typePair);
            }
            else if (IsEnumerableToEnumerable(typePair))
            {
                EmitEnumerableToEnumerable(parentType, typeBuilder, typePair);
            }

            var rootMapper = (Mapper)Activator.CreateInstance(typeBuilder.CreateType());

            _mapperCache.ReplaceStub(typePair, rootMapper);
            rootMapper.AddMappers(_mapperCache.Mappers);

            return rootMapper;
        }

        /// <summary>
        /// Builds a mapper for the specified parent type pair and mapping member.
        /// </summary>
        /// <param name="parentTypePair">The parent type pair.</param>
        /// <param name="mappingMember">The mapping member.</param>
        /// <returns>The built mapper.</returns>
        protected override Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(mappingMember.TypePair);
        }

        /// <summary>
        /// Determines whether the specified type pair is supported for mapping.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns><c>true</c> if the type pair is supported; otherwise, <c>false</c>.</returns>
        protected override bool IsSupportedCore(TypePair typePair)
        {
            return typePair.IsEnumerableTypes;
        }

        private static bool IsDictionaryToDictionary(TypePair typePair)
        {
            return typePair.Source.IsDictionaryOf() && typePair.Target.IsDictionaryOf();
        }

        private static bool IsIEnumerableToArray(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsArray;
        }

        private static bool IsIEnumerableToList(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsListOf();
        }

        private bool IsEnumerableToEnumerable(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsIEnumerable();
        }

        private MapperCacheItem CreateMapperCacheItem(TypePair typePair)
        {
            var mapperCacheItemMaybe = _mapperCache.Get(typePair);
            if (mapperCacheItemMaybe.HasValue)
            {
                return mapperCacheItemMaybe.Value;
            }

            MapperBuilder mapperBuilder = GetMapperBuilder(typePair);
            Mapper mapper = mapperBuilder.Build(typePair);
            MapperCacheItem mapperCacheItem = _mapperCache.Add(typePair, mapper);
            return mapperCacheItem;
        }

        private void EmitConvertItem(TypeBuilder typeBuilder, TypePair typePair, string methodName = ConvertItemMethod)
        {
            MapperCacheItem mapperCacheItem = CreateMapperCacheItem(typePair);

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typeof(object), new[] { typeof(object) });

            IEmitterType sourceMember = EmitArgument.Load(typeof(object), 1);
            IEmitterType targetMember = EmitNull.Load();

            IEmitterType callMapMethod = mapperCacheItem.EmitMapMethod(sourceMember, targetMember);

            EmitReturn.Return(callMapMethod).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        private void EmitDictionaryToDictionary(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            EmitDictionaryToTarget(parentType, typeBuilder, typePair, DictionaryToDictionaryMethod, DictionaryToDictionaryTemplateMethod);
        }

        private void EmitDictionaryToTarget(
            Type parentType,
            TypeBuilder typeBuilder,
            TypePair typePair,
            string methodName,
            string templateMethodName)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typePair.Target, new[] { typeof(IEnumerable) });

            KeyValuePair<Type, Type> sourceTypes = typePair.Source.GetDictionaryItemTypes();
            KeyValuePair<Type, Type> targetTypes = typePair.Target.GetDictionaryItemTypes();

            EmitConvertItem(typeBuilder, new TypePair(sourceTypes.Key, targetTypes.Key), ConvertItemKeyMethod);
            EmitConvertItem(typeBuilder, new TypePair(sourceTypes.Value, targetTypes.Value));

            var arguments = new[] { sourceTypes.Key, sourceTypes.Value, targetTypes.Key, targetTypes.Value };
            MethodInfo methodTemplate = parentType.GetGenericMethod(templateMethodName, arguments);

            IEmitterType returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(typeof(IEnumerable), 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        private void EmitEnumerableToArray(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToArrayMethod, EnumerableToArrayTemplateMethod);
        }

        private void EmitEnumerableToList(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);
            var templateMethod = collectionItemTypePair.IsDeepCloneable ? EnumerableOfDeepCloneableToListTemplateMethod : EnumerableToListTemplateMethod;

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToListMethod, templateMethod);
        }

        private void EmitEnumerableToEnumerable(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);
            var templateMethod = collectionItemTypePair.IsDeepCloneable ? EnumerableOfDeepCloneableToListTemplateMethod : EnumerableToListTemplateMethod;

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToListMethod, templateMethod);
        }

        private static TypePair GetCollectionItemTypePair(TypePair typePair)
        {
            Type sourceItemType = typePair.Source.GetCollectionItemType();
            Type targetItemType = typePair.Target.GetCollectionItemType();

            return new TypePair(sourceItemType, targetItemType);
        }

        private void EmitEnumerableToTarget(
            Type parentType,
            TypeBuilder typeBuilder,
            TypePair typePair,
            TypePair collectionItemTypePair,
            string methodName,
            string templateMethodName)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typePair.Target, new[] { typeof(IEnumerable) });

            EmitConvertItem(typeBuilder, collectionItemTypePair);

            MethodInfo methodTemplate = parentType.GetGenericMethod(templateMethodName, collectionItemTypePair.Target);

            IEmitterType returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(typeof(IEnumerable), 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }
    }
}
