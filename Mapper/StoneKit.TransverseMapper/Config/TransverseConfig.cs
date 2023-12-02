using StoneKit.TransverseMapper.MapperBuilder;

using System.Reflection.Mapping;

namespace StoneKit.TransverseMapper.Config
{
    /// <summary>
    /// Represents the configuration settings for the Transverse mapper.
    /// </summary>
    internal sealed class TransverseConfig : ITransverseConfig
    {
        private readonly TargetMapperBuilder _targetMapperBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseConfig"/> class.
        /// </summary>
        /// <param name="targetMapperBuilder">The target mapper builder to be configured.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided targetMapperBuilder is null.</exception>
        public TransverseConfig(TargetMapperBuilder targetMapperBuilder)
        {
            _targetMapperBuilder = targetMapperBuilder ?? throw new ArgumentNullException(nameof(targetMapperBuilder));
        }

        /// <summary>
        /// Sets the name matching function for property and field mapping.
        /// </summary>
        /// <param name="nameMatching">The function to determine if two names match.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided nameMatching function is null.</exception>
        public void NameMatching(Func<string, string, bool> nameMatching)
        {
            if (nameMatching == null)
            {
                throw new ArgumentNullException(nameof(nameMatching));
            }

            _targetMapperBuilder.SetNameMatching(nameMatching);
        }

        /// <summary>
        /// Resets the name matching function to the default.
        /// </summary>
        public void Reset()
        {
            _targetMapperBuilder.SetNameMatching(TargetMapperBuilder.DefaultNameMatching);
        }
    }
}
