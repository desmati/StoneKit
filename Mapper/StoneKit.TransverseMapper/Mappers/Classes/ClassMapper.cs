using System;

namespace StoneKit.TransverseMapper.Mappers.Classes
{
    /// <summary>
    /// Base class for mapping between source and target classes.
    /// </summary>
    /// <typeparam name="TSource">The type of the source class.</typeparam>
    /// <typeparam name="TTarget">The type of the target class.</typeparam>
    internal abstract class ClassMapper<TSource, TTarget> : MapperOf<TSource, TTarget>
    {
        /// <summary>
        /// Creates a new instance of the target class.
        /// </summary>
        /// <returns>The newly created target class instance.</returns>
        protected virtual TTarget CreateTargetInstance()
        {
            throw new NotImplementedException("CreateTargetInstance method is not implemented.");
        }

        /// <summary>
        /// Maps properties from the source class to the target class.
        /// </summary>
        /// <param name="source">The source class instance.</param>
        /// <param name="target">The target class instance.</param>
        /// <returns>The mapped target class instance.</returns>
        protected abstract TTarget MapClass(TSource source, TTarget target);

        /// <summary>
        /// Overrides the base class's core mapping method to handle target instance creation and class mapping.
        /// </summary>
        /// <param name="source">The source class instance.</param>
        /// <param name="target">The target class instance to be mapped.</param>
        /// <returns>The mapped target class instance.</returns>
        protected override TTarget MapCore(TSource source, TTarget target)
        {
            if (target == null)
            {
                // If the target instance is not provided, create a new one.
                target = CreateTargetInstance();
            }

            // Map properties from the source class to the target class.
            TTarget result = MapClass(source, target);

            return result;
        }
    }
}
