using StoneKit.TransverseMapper.Mappers.Builder;
using System.Reflection.Mapping;

namespace StoneKit.TransverseMapper.Config
{
    /// <summary>
    /// Configuration class for the Transverse Mapper in StoneKit.
    /// </summary>
    internal sealed class TransverseConfig : ITransverseConfig
    {
        private readonly TargetMapperBuilder _targetMapperBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseConfig"/> class.
        /// </summary>
        /// <param name="targetMapperBuilder">The TargetMapperBuilder instance to be configured.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="targetMapperBuilder"/> is null.</exception>
        public TransverseConfig(TargetMapperBuilder targetMapperBuilder)
        {
            _targetMapperBuilder = targetMapperBuilder ?? throw new ArgumentNullException(nameof(targetMapperBuilder));
        }

        /// <summary>
        /// Sets a custom name matching function for the Transverse Mapper.
        /// </summary>
        /// <param name="nameMatching">The function that determines if two property names match.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nameMatching"/> is null.</exception>
        public void NameMatching(Func<string, string, bool> nameMatching)
        {
            if (nameMatching == null)
            {
                throw new ArgumentNullException(nameof(nameMatching));
            }
            _targetMapperBuilder.SetNameMatching(nameMatching);
        }

        /// <summary>
        /// Resets the name matching function to the default one.
        /// </summary>
        public void Reset()
        {
            _targetMapperBuilder.SetNameMatching(TargetMapperBuilder.DefaultNameMatching);
        }
    }
}
