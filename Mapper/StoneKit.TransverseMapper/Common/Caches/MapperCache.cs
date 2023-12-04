using StoneKit.TransverseMapper.Mappers;

using System;
using System.Collections.Concurrent;

namespace StoneKit.TransverseMapper.Common.Caches
{
    /// <summary>
    /// Cache for storing and managing mappers in the TransverseMapper framework.
    /// </summary>
    internal sealed class MapperCache
    {
        private readonly Dictionary<TypePair, MapperCacheItem> _cache = new Dictionary<TypePair, MapperCacheItem>();

        /// <summary>
        /// Gets a value indicating whether the cache is empty.
        /// </summary>
        public bool IsEmpty => _cache.Count == 0;

        /// <summary>
        /// Gets a list of all mappers in the cache, ordered by their IDs.
        /// </summary>
        public List<Mapper> Mappers
        {
            get
            {
                return _cache.Values
                             .OrderBy(x => x.Id)
                             .Select(x => x.Mapper)
                             .ToList();
            }
        }

        /// <summary>
        /// Gets a list of all MapperCacheItems in the cache.
        /// </summary>
        public List<MapperCacheItem> MapperCacheItems => _cache.Values.ToList();

        /// <summary>
        /// Adds a stub to the cache for the specified key (TypePair).
        /// </summary>
        /// <param name="key">The key (TypePair) for which to add a stub.</param>
        /// <returns>The created or existing MapperCacheItem associated with the key.</returns>
        public MapperCacheItem AddStub(TypePair key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }

            var mapperCacheItem = new MapperCacheItem { Id = GetId() };
            _cache.TryAdd(key, mapperCacheItem);

            return mapperCacheItem;
        }

        /// <summary>
        /// Replaces a stub in the cache with the specified key (TypePair) with a fully initialized Mapper.
        /// </summary>
        /// <param name="key">The key (TypePair) for which to replace the stub.</param>
        /// <param name="mapper">The fully initialized Mapper to replace the stub with.</param>
        public void ReplaceStub(TypePair key, Mapper mapper)
        {
            _cache[key].Mapper = mapper;
        }

        /// <summary>
        /// Adds a fully initialized Mapper to the cache with the specified key (TypePair).
        /// </summary>
        /// <param name="key">The key (TypePair) for which to add the Mapper.</param>
        /// <param name="mapper">The fully initialized Mapper to be added.</param>
        /// <returns>The created or existing MapperCacheItem associated with the key.</returns>
        public MapperCacheItem Add(TypePair key, Mapper mapper)
        {
            MapperCacheItem result;
            if (_cache.TryGetValue(key, out result!))
            {
                return result;
            }

            result = new MapperCacheItem
            {
                Id = GetId(),
                Mapper = mapper
            };

            _cache.TryAdd(key, result);

            return result;
        }

        /// <summary>
        /// Gets a Maybe containing the MapperCacheItem associated with the specified key (TypePair).
        /// </summary>
        /// <param name="key">The key (TypePair) for which to get the MapperCacheItem.</param>
        /// <returns>A Maybe containing the MapperCacheItem if found; otherwise, an empty Maybe.</returns>
        public Maybe<MapperCacheItem> Get(TypePair key)
        {
            MapperCacheItem result;
            if (_cache.TryGetValue(key, out result!))
            {
                return new Maybe<MapperCacheItem>(result);
            }

            return Maybe<MapperCacheItem>.Empty;
        }

        private int GetId()
        {
            return _cache.Count;
        }
    }
}
