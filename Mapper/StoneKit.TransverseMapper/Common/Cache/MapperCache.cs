using StoneKit.TransverseMapper.Mappers;

using System.Collections.Concurrent;

namespace StoneKit.TransverseMapper.Common.Cache
{
    /// <summary>
    /// Represents a cache for storing mappers based on type pairs.
    /// </summary>
    internal sealed class MapperCache
    {
        private readonly ConcurrentDictionary<TypePair, MapperCacheItem> _cache = new ConcurrentDictionary<TypePair, MapperCacheItem>();

        /// <summary>
        /// Gets a value indicating whether the cache is empty.
        /// </summary>
        public bool IsEmpty => _cache.Count == 0;

        /// <summary>
        /// Gets the list of mappers in the cache, ordered by their identifier.
        /// </summary>
        public List<MapperBase> Mappers
        {
            get
            {
                return _cache.ToArray()
                    .Select(x=> x.Value.Mapper)
                    .ToList();
            }
        }

        /// <summary>
        /// Gets the list of mapper cache items in the cache.
        /// </summary>
        public List<MapperCacheItem> MapperCacheItems => _cache.Values.ToList();

        /// <summary>
        /// Adds a stub for the specified type pair to the cache.
        /// </summary>
        /// <param name="key">The type pair key.</param>
        /// <returns>The added or existing mapper cache item.</returns>
        public MapperCacheItem AddStub(TypePair key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }

            var mapperCacheItem = new MapperCacheItem { Id = GetId() };
            _cache[key] = mapperCacheItem;
            return mapperCacheItem;
        }

        /// <summary>
        /// Replaces a stub for the specified type pair with the actual mapper.
        /// </summary>
        /// <param name="key">The type pair key.</param>
        /// <param name="mapper">The actual mapper to replace the stub.</param>
        public void ReplaceStub(TypePair key, MapperBase mapper)
        {
            _cache[key].Mapper = mapper;
        }

        /// <summary>
        /// Adds a mapper to the cache for the specified type pair.
        /// </summary>
        /// <param name="key">The type pair key.</param>
        /// <param name="mapper">The mapper to add to the cache.</param>
        /// <returns>The added mapper cache item.</returns>
        public MapperCacheItem Add(TypePair key, MapperBase mapper)
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
        /// Gets the mapper cache item for the specified type pair if it exists.
        /// </summary>
        /// <param name="key">The type pair key.</param>
        /// <returns>The mapper cache item if found, otherwise an empty result.</returns>
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
