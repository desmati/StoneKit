namespace StoneKit.TransverseMapper.Mappers
{
    /// <summary>
    /// Abstract base class for mappers.
    /// </summary>
    internal abstract class Mapper
    {
        /// <summary>
        /// The name of the Map method used for mapping.
        /// </summary>
        public const string MapMethodName = "Map";

        /// <summary>
        /// The name of the field storing an array of mappers in the derived classes.
        /// </summary>
        public const string MappersFieldName = "_mappers";

        /// <summary>
        /// Array of mappers used in mapping operations.
        /// </summary>
        protected Mapper[] _mappers = null;

        /// <summary>
        /// Adds an array of mappers to the current instance.
        /// </summary>
        /// <param name="mappers">The array of mappers to add.</param>
        public void AddMappers(IEnumerable<Mapper> mappers)
        {
            _mappers = mappers.ToArray();
        }

        /// <summary>
        /// Updates the root mapper at the specified index with a new mapper.
        /// </summary>
        /// <param name="mapperId">The index of the root mapper to update.</param>
        /// <param name="mapper">The new mapper to replace the existing one.</param>
        public void UpdateRootMapper(int mapperId, Mapper mapper)
        {
            if (_mappers == null)
            {
                return;
            }

            for (int i = 0; i < _mappers.Length; i++)
            {
                if (i == mapperId)
                {
                    if (_mappers[i] == null)
                    {
                        _mappers[i] = mapper;
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Maps the source object to the target object using the implemented mapping logic.
        /// </summary>
        /// <param name="source">The source object to map from.</param>
        /// <param name="target">The optional target object to map to.</param>
        /// <returns>The result of the mapping operation.</returns>
        public object Map(object source, object target = null)
        {
            return MapCore(source, target);
        }

        /// <summary>
        /// Performs the core mapping operation from the source object to the target object.
        /// </summary>
        /// <param name="source">The source object to map from.</param>
        /// <param name="target">The optional target object to map to.</param>
        /// <returns>The result of the mapping operation.</returns>
        protected abstract object MapCore(object source, object target);
    }
}
