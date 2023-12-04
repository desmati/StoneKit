namespace StoneKit.TransverseMapper.Mappers
{
    /// <summary>
    /// Abstract base class for mappers that map from a source type to a target type.
    /// </summary>
    /// <typeparam name="TSource">The source type of the mapper.</typeparam>
    /// <typeparam name="TTarget">The target type of the mapper.</typeparam>
    internal abstract class MapperOf<TSource, TTarget> : Mapper
    {
        /// <summary>
        /// Overrides the base class method to perform the core mapping operation.
        /// </summary>
        /// <param name="source">The source object to map from.</param>
        /// <param name="target">The target object to map to.</param>
        /// <returns>The result of the mapping operation.</returns>
        protected override object MapCore(object source, object target)
        {
            if (source == null)
            {
                return default(TTarget);
            }
            return MapCore((TSource)source, (TTarget)target);
        }

        /// <summary>
        /// Performs the core mapping operation from the source type to the target type.
        /// </summary>
        /// <param name="source">The source object to map from.</param>
        /// <param name="target">The target object to map to.</param>
        /// <returns>The result of the mapping operation.</returns>
        protected abstract TTarget MapCore(TSource source, TTarget target);
    }
}
