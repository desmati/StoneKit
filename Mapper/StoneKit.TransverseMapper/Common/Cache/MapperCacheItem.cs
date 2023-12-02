using StoneKit.TransverseMapper.Mappers;

using System.Reflection;

namespace StoneKit.TransverseMapper.Common.Cache
{
    /// <summary>
    /// Represents an item stored in the <see cref="MapperCache"/> containing a unique identifier and a mapper instance.
    /// </summary>
    internal sealed class MapperCacheItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the mapper cache item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the mapper instance associated with the cache item.
        /// </summary>
        public MapperBase Mapper { get; set; } = null!;

        /// <summary>
        /// Emits the mapping method for the associated mapper.
        /// </summary>
        /// <param name="sourceMember">The emitter type representing the source member.</param>
        /// <param name="targetMember">The emitter type representing the target member.</param>
        /// <returns>The emitter type representing the result of the mapping method.</returns>
        public IEmitterType EmitMapMethod(IEmitterType sourceMember, IEmitterType targetMember)
        {
            Type mapperType = typeof(MapperBase);
            MethodInfo mapMethod = mapperType.GetMethod(MapperBase.MapMethodName, BindingFlags.Instance | BindingFlags.Public)!;
            FieldInfo mappersField = mapperType.GetField(MapperBase.MappersFieldName, BindingFlags.Instance | BindingFlags.NonPublic)!;
            IEmitterType mappers = EmitField.Load(EmitThis.Load(mapperType), mappersField);
            IEmitterType mapper = EmitArray.Load(mappers, Id);
            IEmitterType result = EmitMethod.Call(mapMethod, mapper, sourceMember, targetMember);
            return result;
        }
    }
}
