using StoneKit.TransverseMapper.Mappers;

using System.Reflection;

namespace StoneKit.TransverseMapper.Common.Caches
{
    /// <summary>
    /// Represents an item stored in the MapperCache, containing an ID and a Mapper instance.
    /// </summary>
    internal sealed class MapperCacheItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for the MapperCacheItem.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Mapper instance associated with the MapperCacheItem.
        /// </summary>
        public Mapper Mapper { get; set; } = null;

        /// <summary>
        /// Emits the map method for the Mapper associated with this MapperCacheItem.
        /// </summary>
        /// <param name="sourceMember">The source member for mapping.</param>
        /// <param name="targetMember">The target member for mapping.</param>
        /// <returns>The result of the emitted map method.</returns>
        public IEmitterType EmitMapMethod(IEmitterType sourceMember, IEmitterType targetMember)
        {
            Type mapperType = typeof(Mapper);
            MethodInfo mapMethod = mapperType.GetMethod(Mapper.MapMethodName, BindingFlags.Instance | BindingFlags.Public);
            FieldInfo mappersField = mapperType.GetField(Mapper.MappersFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            IEmitterType mappers = EmitField.Load(EmitThis.Load(mapperType), mappersField);
            IEmitterType mapper = EmitArray.Load(mappers, Id);
            IEmitterType result = EmitMethod.Call(mapMethod, mapper, sourceMember, targetMember);
            return result;
        }
    }
}
