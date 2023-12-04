using StoneKit.TransverseMapper.Mappers.Classes.Members;

using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Builder
{
    /// <summary>
    /// Abstract base class for building mappers in the TransverseMapper framework.
    /// </summary>
    internal abstract class MapperBuilder
    {
        /// <summary>
        /// MethodAttributes for protected virtual methods overriding.
        /// </summary>
        protected const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;

        /// <summary>
        /// Default assembly name for dynamic mappers.
        /// </summary>
        private const string AssemblyName = "DynamicTransverse";

        /// <summary>
        /// The dynamic assembly used for mapping.
        /// </summary>
        protected readonly IDynamicAssembly _assembly;

        /// <summary>
        /// Configuration for the mapper builder.
        /// </summary>
        protected readonly IMapperBuilderConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapperBuilder"/> class.
        /// </summary>
        /// <param name="config">The configuration for the mapper builder.</param>
        protected MapperBuilder(IMapperBuilderConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _assembly = config.Assembly;
        }

        /// <summary>
        /// Gets the name of the scope for the mapper.
        /// </summary>
        protected abstract string ScopeName { get; }

        /// <summary>
        /// Builds a mapper for the specified type pair.
        /// </summary>
        /// <param name="typePair">The pair of types to build the mapper for.</param>
        /// <returns>The built mapper instance.</returns>
        public Mapper Build(TypePair typePair)
        {
            return BuildCore(typePair);
        }

        /// <summary>
        /// Builds a mapper for the specified parent type pair and mapping member.
        /// </summary>
        /// <param name="parentTypePair">The pair of types representing the parent.</param>
        /// <param name="mappingMember">The mapping member for which to build the mapper.</param>
        /// <returns>The built mapper instance.</returns>
        public Mapper Build(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(parentTypePair, mappingMember);
        }

        /// <summary>
        /// Checks if the specified type pair is supported by the mapper builder.
        /// </summary>
        /// <param name="typePair">The pair of types to check for support.</param>
        /// <returns>True if the type pair is supported; otherwise, false.</returns>
        public bool IsSupported(TypePair typePair)
        {
            return IsSupportedCore(typePair);
        }

        /// <summary>
        /// Builds a mapper for the specified type pair.
        /// </summary>
        /// <param name="typePair">The pair of types to build the mapper for.</param>
        /// <returns>The built mapper instance.</returns>
        protected abstract Mapper BuildCore(TypePair typePair);

        /// <summary>
        /// Builds a mapper for the specified parent type pair and mapping member.
        /// </summary>
        /// <param name="parentTypePair">The pair of types representing the parent.</param>
        /// <param name="mappingMember">The mapping member for which to build the mapper.</param>
        /// <returns>The built mapper instance.</returns>
        protected abstract Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember);

        /// <summary>
        /// Gets a specific mapper builder for the specified type pair.
        /// </summary>
        /// <param name="typePair">The pair of types for which to get the mapper builder.</param>
        /// <returns>The mapper builder instance.</returns>
        protected MapperBuilder GetMapperBuilder(TypePair typePair)
        {
            return _config.GetMapperBuilder(typePair);
        }

        /// <summary>
        /// Generates a unique name for the mapper within the assembly scope.
        /// </summary>
        /// <returns>The generated mapper name.</returns>
        protected string GetMapperFullName()
        {
            string random = Guid.NewGuid().ToString("N");
            return $"{AssemblyName}.{ScopeName}.Mapper{random}";
        }

        /// <summary>
        /// Checks if the specified type pair is supported by the mapper builder.
        /// </summary>
        /// <param name="typePair">The pair of types to check for support.</param>
        /// <returns>True if the type pair is supported; otherwise, false.</returns>
        protected abstract bool IsSupportedCore(TypePair typePair);
    }
}
