namespace StoneKit.TransverseMapper.Mappers
{
    /// <summary>
    /// Represents the base class for mappers in the Transverse framework.
    /// </summary>
    internal abstract class MapperBase
    {
        /// <summary>
        /// The name of the map method used by derived mapper classes.
        /// </summary>
        public const string MapMethodName = "Map";

        /// <summary>
        /// The name of the field storing mappers in the derived mapper classes.
        /// </summary>
        public const string MappersFieldName = "_mappers";

        /// <summary>
        /// Array of mappers used by the derived mapper classes.
        /// </summary>
        protected MapperBase[] _mappers = null!;

        /// <summary>
        /// Adds an array of mappers to the current mapper instance.
        /// </summary>
        /// <param name="mappers">An enumerable collection of mappers to be added.</param>
        public void AddMappers(IEnumerable<MapperBase> mappers)
        {
            _mappers = mappers.ToArray();
        }

        /// <summary>
        /// Updates the root mapper at the specified index with the provided mapper.
        /// </summary>
        /// <param name="mapperId">The index of the mapper to be updated.</param>
        /// <param name="mapper">The new mapper to replace the existing one.</param>
        public void UpdateRootMapper(int mapperId, MapperBase mapper)
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
        /// Maps the source object to the target object using the core mapping logic.
        /// </summary>
        /// <param name="source">The source object to be mapped.</param>
        /// <param name="target">The optional target object to map into. If null, a new instance will be created.</param>
        /// <returns>The mapped object.</returns>
        public object Map(object source, object target = null!)
        {
            return MapCore(source, target);
        }

        /// <summary>
        /// Performs the core mapping logic for the derived mapper classes.
        /// </summary>
        /// <param name="source">The source object to be mapped.</param>
        /// <param name="target">The optional target object to map into. If null, a new instance will be created.</param>
        /// <returns>The mapped object.</returns>
        protected abstract object MapCore(object source, object target);
    }
}
