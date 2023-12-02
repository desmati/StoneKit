using StoneKit.TransverseMapper.Mappers;
using StoneKit.TransverseMapper.Mappers.Members;

using System.Reflection;

namespace StoneKit.TransverseMapper.MapperBuilder
{
    /// <summary>
    /// Represents an abstract base class for building mappers in the Transverse library.
    /// </summary>
    internal abstract class MapperBuilderBase
    {
        /// <summary>
        /// The default method attributes for protected and virtual methods.
        /// </summary>
        protected const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;

        private const string AssemblyName = "DynamicTransverse";
        protected readonly IDynamicAssembly _assembly;
        protected readonly IMapperBuilderConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapperBuilder"/> class.
        /// </summary>
        /// <param name="config">The configuration for the mapper builder.</param>
        protected MapperBuilderBase(IMapperBuilderConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _assembly = config.Assembly;
        }

        /// <summary>
        /// Gets the name of the scope for the mapper builder.
        /// </summary>
        protected abstract string ScopeName { get; }

        /// <summary>
        /// Builds a mapper for the specified type pair.
        /// </summary>
        /// <param name="typePair">The source and target types.</param>
        /// <returns>The built mapper.</returns>
        public MapperBase Build(TypePair typePair)
        {
            return BuildCore(typePair);
        }

        /// <summary>
        /// Builds a mapper for the specified parent type pair and mapping member.
        /// </summary>
        /// <param name="parentTypePair">The parent source and target types.</param>
        /// <param name="mappingMember">The mapping member.</param>
        /// <returns>The built mapper.</returns>
        public MapperBase Build(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(parentTypePair, mappingMember);
        }

        /// <summary>
        /// Checks if the mapper builder supports the specified type pair.
        /// </summary>
        /// <param name="typePair">The source and target types.</param>
        /// <returns><c>true</c> if the type pair is supported; otherwise, <c>false</c>.</returns>
        public bool IsSupported(TypePair typePair)
        {
            return IsSupportedCore(typePair);
        }

        /// <summary>
        /// Builds a mapper for the specified type pair in the derived class.
        /// </summary>
        /// <param name="typePair">The source and target types.</param>
        /// <returns>The built mapper.</returns>
        protected abstract MapperBase BuildCore(TypePair typePair);

        /// <summary>
        /// Builds a mapper for the specified parent type pair and mapping member in the derived class.
        /// </summary>
        /// <param name="parentTypePair">The parent source and target types.</param>
        /// <param name="mappingMember">The mapping member.</param>
        /// <returns>The built mapper.</returns>
        protected abstract MapperBase BuildCore(TypePair parentTypePair, MappingMember mappingMember);

        /// <summary>
        /// Checks if the mapper builder supports the specified type pair in the derived class.
        /// </summary>
        /// <param name="typePair">The source and target types.</param>
        /// <returns><c>true</c> if the type pair is supported; otherwise, <c>false</c>.</returns>
        protected abstract bool IsSupportedCore(TypePair typePair);

        /// <summary>
        /// Gets the appropriate mapper builder for the specified type pair from the configuration.
        /// </summary>
        /// <param name="typePair">The source and target types.</param>
        /// <returns>The mapper builder for the specified type pair.</returns>
        protected MapperBuilderBase GetMapperBuilder(TypePair typePair)
        {
            return _config.GetMapperBuilder(typePair);
        }

        /// <summary>
        /// Gets a unique name for the mapper.
        /// </summary>
        /// <returns>A unique name for the mapper.</returns>
        protected string GetMapperFullName()
        {
            string random = Guid.NewGuid().ToString("N");
            return $"{AssemblyName}.{ScopeName}.Mapper{random}";
        }
    }
}
