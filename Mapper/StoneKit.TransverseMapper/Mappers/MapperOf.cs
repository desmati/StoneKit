namespace StoneKit.TransverseMapper.Mappers
{
    /// <summary>
    /// Represents the base class for mappers in the Transverse framework with specific source and target types.
    /// </summary>
    /// <typeparam name="TSource">The source type for the mapper.</typeparam>
    /// <typeparam name="TTarget">The target type for the mapper.</typeparam>
    internal abstract class MapperOf<TSource, TTarget> : MapperBase
    {
        /// <summary>
        /// Maps the source object to the target object using the core mapping logic with specific source and target types.
        /// </summary>
        /// <param name="source">The source object to be mapped.</param>
        /// <param name="target">The optional target object to map into. If null, a new instance will be created.</param>
        /// <returns>The mapped object.</returns>
        protected override object MapCore(object source, object target)
        {
            if (source == null)
            {
                return default(TTarget)!;
            }

            return MapCore((TSource)source!, (TTarget)target!)!;
        }

        /// <summary>
        /// Performs the core mapping logic for the derived mapper classes with specific source and target types.
        /// </summary>
        /// <param name="source">The source object of type <typeparamref name="TSource"/> to be mapped.</param>
        /// <param name="target">The optional target object of type <typeparamref name="TTarget"/> to map into. If null, a new instance will be created.</param>
        /// <returns>The mapped object of type <typeparamref name="TTarget"/>.</returns>
        protected abstract TTarget MapCore(TSource source, TTarget target);
    }
}
