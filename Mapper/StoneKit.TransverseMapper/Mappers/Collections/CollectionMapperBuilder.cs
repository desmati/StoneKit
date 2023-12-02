using StoneKit.TransverseMapper.Common.Cache;
using StoneKit.TransverseMapper.MapperBuilder;
using StoneKit.TransverseMapper.Mappers.Members;

using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace StoneKit.TransverseMapper.Mappers.Collections
{
    /// <summary>
    /// Builder for creating mappers for collection types.
    /// </summary>
    internal sealed class CollectionMapperBuilder : MapperBuilderBase
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
        /// Builds the core functionality of the collection mapper.
        /// </summary>
        /// <param name="typePair">The type pair for the collection.</param>
        /// <returns>The built mapper.</returns>
        protected override MapperBase BuildCore(TypePair typePair)
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

            var rootMapper = (MapperBase)Activator.CreateInstance(typeBuilder.CreateTypeInfo().AsType())!;

            _mapperCache.ReplaceStub(typePair, rootMapper);
            rootMapper.AddMappers(_mapperCache.Mappers);

            return rootMapper;
        }

        /// <summary>
        /// Builds the core functionality of the collection mapper for mapping members.
        /// </summary>
        /// <param name="parentTypePair">The parent type pair.</param>
        /// <param name="mappingMember">The mapping member.</param>
        /// <returns>The built mapper.</returns>
        protected override MapperBase BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(mappingMember.TypePair);
        }

        /// <summary>
        /// Determines if the given type pair is supported for building a collection mapper.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>True if supported; otherwise, false.</returns>
        protected override bool IsSupportedCore(TypePair typePair)
        {
            return typePair.IsEnumerableTypes;
        }

        /// <summary>
        /// Determines if the type pair represents a dictionary to dictionary mapping.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>True if dictionary to dictionary mapping; otherwise, false.</returns>
        private static bool IsDictionaryToDictionary(TypePair typePair)
        {
            return typePair.Source.IsDictionaryOf() && typePair.Target.IsDictionaryOf();
        }

        /// <summary>
        /// Determines if the type pair represents an IEnumerable to array mapping.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>True if IEnumerable to array mapping; otherwise, false.</returns>
        private static bool IsIEnumerableToArray(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsArray;
        }

        /// <summary>
        /// Determines if the type pair represents an IEnumerable to list mapping.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>True if IEnumerable to list mapping; otherwise, false.</returns>
        private static bool IsIEnumerableToList(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsListOf();
        }

        /// <summary>
        /// Determines if the type pair represents an IEnumerable to IEnumerable mapping.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>True if IEnumerable to IEnumerable mapping; otherwise, false.</returns>
        private bool IsEnumerableToEnumerable(TypePair typePair)
        {
            return typePair.Source.IsIEnumerable() && typePair.Target.IsIEnumerable();
        }

        /// <summary>
        /// Creates a mapper cache item for the given type pair.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>The mapper cache item.</returns>
        private MapperCacheItem CreateMapperCacheItem(TypePair typePair)
        {
            var mapperCacheItemOption = _mapperCache.Get(typePair);
            if (mapperCacheItemOption.HasValue)
            {
                return mapperCacheItemOption.Value;
            }

            MapperBuilderBase mapperBuilder = GetMapperBuilder(typePair);
            MapperBase mapper = mapperBuilder.Build(typePair);
            MapperCacheItem mapperCacheItem = _mapperCache.Add(typePair, mapper);
            return mapperCacheItem;
        }

        /// <summary>
        /// Emits the conversion of an item using the specified method name.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="typePair">The type pair.</param>
        /// <param name="methodName">The method name.</param>
        private void EmitConvertItem(TypeBuilder typeBuilder, TypePair typePair, string methodName = ConvertItemMethod)
        {
            MapperCacheItem mapperCacheItem = CreateMapperCacheItem(typePair);

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, OverrideProtected, typeof(object), new[] { typeof(object) });

            IEmitterType sourceMember = EmitArgument.Load(typeof(object), 1);
            IEmitterType targetMember = EmitNull.Load();

            IEmitterType callMapMethod = mapperCacheItem.EmitMapMethod(sourceMember, targetMember);

            EmitReturn.Return(callMapMethod).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        /// <summary>
        /// Emits the dictionary to dictionary conversion.
        /// </summary>
        /// <param name="parentType">The parent type.</param>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="typePair">The type pair.</param>
        private void EmitDictionaryToDictionary(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            EmitDictionaryToTarget(parentType, typeBuilder, typePair, DictionaryToDictionaryMethod, DictionaryToDictionaryTemplateMethod);
        }

        /// <summary>
        /// Emits the dictionary to target conversion.
        /// </summary>
        /// <param name="parentType">The parent type.</param>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="typePair">The type pair.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="templateMethodName">The template method name.</param>
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
            MethodInfo methodTemplate = parentType.GetGenericMethod(templateMethodName, arguments)!;

            IEmitterType returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(typeof(IEnumerable), 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }

        /// <summary>
        /// Emits the IEnumerable to array conversion.
        /// </summary>
        /// <param name="parentType">The parent type.</param>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="typePair">The type pair.</param>
        private void EmitEnumerableToArray(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToArrayMethod, EnumerableToArrayTemplateMethod);
        }


        /// <summary>
        /// Emits the IEnumerable to list conversion.
        /// </summary>
        /// <param name="parentType">The parent type.</param>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="typePair">The type pair.</param>
        private void EmitEnumerableToList(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);
            var templateMethod = collectionItemTypePair.IsDeepCloneable ? EnumerableOfDeepCloneableToListTemplateMethod : EnumerableToListTemplateMethod;

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToListMethod, templateMethod);
        }

        /// <summary>
        /// Emits the IEnumerable to IEnumerable conversion.
        /// </summary>
        /// <param name="parentType">The parent type.</param>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="typePair">The type pair.</param>
        private void EmitEnumerableToEnumerable(Type parentType, TypeBuilder typeBuilder, TypePair typePair)
        {
            var collectionItemTypePair = GetCollectionItemTypePair(typePair);
            var templateMethod = collectionItemTypePair.IsDeepCloneable ? EnumerableOfDeepCloneableToListTemplateMethod : EnumerableToListTemplateMethod;

            EmitEnumerableToTarget(parentType, typeBuilder, typePair, collectionItemTypePair, EnumerableToListMethod, templateMethod);
        }

        /// <summary>
        /// Gets the collection item type pair.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>The collection item type pair.</returns>
        private static TypePair GetCollectionItemTypePair(TypePair typePair)
        {
            Type sourceItemType = typePair.Source.GetCollectionItemType()!;
            Type targetItemType = typePair.Target.GetCollectionItemType()!;

            return new TypePair(sourceItemType, targetItemType);
        }

        /// <summary>
        /// Emits the IEnumerable to target conversion.
        /// </summary>
        /// <param name="parentType">The parent type.</param>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="typePair">The type pair.</param>
        /// <param name="collectionItemTypePair">The collection item type pair.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="templateMethodName">The template method name.</param>
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

            MethodInfo methodTemplate = parentType.GetGenericMethod(templateMethodName, collectionItemTypePair.Target)!;

            IEmitterType returnValue = EmitMethod.Call(methodTemplate, EmitThis.Load(parentType), EmitArgument.Load(typeof(IEnumerable), 1));
            EmitReturn.Return(returnValue).Emit(new CodeGenerator(methodBuilder.GetILGenerator()));
        }
    }
}


