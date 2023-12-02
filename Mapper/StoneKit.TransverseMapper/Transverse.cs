using StoneKit.TransverseMapper.Config;
using StoneKit.TransverseMapper.Config.Bindings;
using StoneKit.TransverseMapper.MapperBuilder;
using StoneKit.TransverseMapper.Mappers;

using System.Collections.Concurrent;

namespace System.Reflection.Mapping
{
    /// <summary>
    /// Transverse is an object-to-object mapper for .NET with a focus on performance.
    /// It allows easy mapping of properties or fields from one object to another.
    /// </summary>
    public static class Transverse
    {
        /// <summary>
        /// The name of the dynamic assembly used by Transverse.
        /// </summary>
        public const string AssemblyName = "StoneKitCoreReflectionDynamicAssemblyName";

        /// <summary>
        /// Gets the dynamic assembly used by Transverse.
        /// </summary>
        public static readonly IDynamicAssembly Assembly = new DynamicAssembly(AssemblyName);

        private static readonly ConcurrentDictionary<TypePair, MapperBase> _mappers = new ConcurrentDictionary<TypePair, MapperBase>();
        private static readonly TargetMapperBuilder _targetMapperBuilder;
        private static readonly TransverseConfig _config;

        static Transverse()
        {
            _targetMapperBuilder = new TargetMapperBuilder(Assembly);
            _config = new TransverseConfig(_targetMapperBuilder);
        }

        /// <summary>
        /// Creates a one-way mapping between source and target types.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <remarks>This method is thread-safe.</remarks>
        public static void Bind<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();
            var mapper = _targetMapperBuilder.Build(typePair);
            _mappers.AddOrUpdate(typePair, mapper, (k, v) => mapper);
        }

        /// <summary>
        /// Creates a one-way mapping between source and target types.
        /// </summary>
        /// <param name="sourceType">The source type.</param>
        /// <param name="targetType">The target type.</param>
        /// <remarks>This method is thread-safe.</remarks>
        public static void Bind(Type sourceType, Type targetType)
        {
            if (sourceType == null)
            {
                throw new ArgumentNullException(nameof(sourceType));
            }

            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            TypePair typePair = TypePair.Create(sourceType, targetType);
            var mapper = _targetMapperBuilder.Build(typePair);
            _mappers.AddOrUpdate(typePair, mapper, (k, v) => mapper);
        }

        /// <summary>
        /// Creates a one-way mapping between source and target types with custom binding configuration.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <param name="config">Binding configuration.</param>
        /// <remarks>This method is thread-safe.</remarks>
        public static void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config)
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            var bindingConfig = new BindingConfigOf<TSource, TTarget>();
            config(bindingConfig);

            var mapper = _targetMapperBuilder.Build(typePair, bindingConfig);
            _mappers.AddOrUpdate(typePair, mapper, (k, v) => mapper);
        }

        /// <summary>
        /// Checks if a binding exists from source to target.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <returns>True if a binding exists, otherwise false.</returns>
        /// <remarks>This method is thread-safe.</remarks>
        public static bool BindingExists<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();
            return _mappers.ContainsKey(typePair);
        }

        /// <summary>
        /// Maps the source object to the target type.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>The mapped object.</returns>
        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target = default!)
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            MapperBase mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source!, target!);

            return result;
        }

        /// <summary>
        /// Maps the source object to the target type.
        /// </summary>
        /// <param name="sourceType">The source type.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>The mapped object.</returns>
        public static object Map(Type sourceType, Type targetType, object source, object target = null!)
        {
            TypePair typePair = TypePair.Create(sourceType, targetType);

            MapperBase mapper = GetMapper(typePair);
            var result = mapper.Map(source, target);

            return result;
        }

        /// <summary>
        /// Configures the Transverse mapper.
        /// </summary>
        /// <param name="config">Lambda expression to provide configuration settings.</param>
        public static void Config(Action<ITransverseConfig> config)
        {
            config(_config);
        }

        /// <summary>
        /// Maps the source object to the target type.
        /// </summary>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <param name="source">The source object [Not null].</param>
        /// <returns>The mapped object.</returns>
        public static TTarget Map<TTarget>(object source)
        {
            if (source.IsNull())
            {
                throw new Exception("Source cannot be null. Use Transverse.Map<TSource, TTarget> method instead.");
            }

            TypePair typePair = TypePair.Create(source.GetType(), typeof(TTarget));

            MapperBase mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source);

            return result;
        }

        private static MapperBase GetMapper(TypePair typePair)
        {
            MapperBase mapper;

            if (_mappers.TryGetValue(typePair, out mapper!) == false)
            {
                throw new TransverseException($"No binding found for '{typePair.Source.Name}' to '{typePair.Target.Name}'. " +
                                              $"Call Transverse.Bind<{typePair.Source.Name}, {typePair.Target.Name}>()");
            }

            return mapper;
        }
    }
}
