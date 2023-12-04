namespace StoneKit.TransverseMapper.Mappers.Classes
{
    /// <summary>
    /// Abstract base class for class mappers.
    /// </summary>
    /// <typeparam name="TSource">The source type of the class mapper.</typeparam>
    /// <typeparam name="TTarget">The target type of the class mapper.</typeparam>
    internal abstract class ClassMapper<TSource, TTarget> : MapperOf<TSource, TTarget>
    {
        /// <summary>
        /// Creates an instance of the target type.
        /// </summary>
        /// <returns>The created instance of the target type.</returns>
        protected virtual TTarget CreateTargetInstance()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs the mapping operation for class types from the source to the target.
        /// </summary>
        /// <param name="source">The source object to map from.</param>
        /// <param name="target">The target object to map to.</param>
        /// <returns>The result of the mapping operation.</returns>
        protected abstract TTarget MapClass(TSource source, TTarget target);

        /// <summary>
        /// Overrides the base class method to perform the core mapping operation for class types.
        /// </summary>
        /// <param name="source">The source object to map from.</param>
        /// <param name="target">The target object to map to.</param>
        /// <returns>The result of the mapping operation.</returns>
        protected override TTarget MapCore(TSource source, TTarget target)
        {
            if (target == null)
            {
                target = CreateTargetInstance();
            }
            TTarget result = MapClass(source, target);
            return result;
        }
    }
}
