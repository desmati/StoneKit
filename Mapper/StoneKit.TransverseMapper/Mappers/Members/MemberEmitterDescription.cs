using StoneKit.TransverseMapper.Common.Cache;

using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Members
{
    /// <summary>
    /// Represents a description of a member emitter with an associated mapper cache.
    /// </summary>
    internal sealed class MemberEmitterDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberEmitterDescription"/> class with the specified emitter and mapper cache.
        /// </summary>
        /// <param name="emitter">The emitter associated with the member.</param>
        /// <param name="mappers">The mapper cache associated with the member.</param>
        public MemberEmitterDescription(IEmitter emitter, MapperCache mappers)
        {
            Emitter = emitter;
            MapperCache = new Maybe<MapperCache>(mappers, mappers.IsEmpty);
        }

        /// <summary>
        /// Gets the emitter associated with the member.
        /// </summary>
        public IEmitter Emitter { get; }

        /// <summary>
        /// Gets or sets the mapper cache associated with the member.
        /// </summary>
        public Maybe<MapperCache> MapperCache { get; private set; }

        /// <summary>
        /// Adds a mapper cache to the member emitter description.
        /// </summary>
        /// <param name="value">The mapper cache to be added.</param>
        public void AddMapper(MapperCache value)
        {
            MapperCache = value.ToMaybe();
        }
    }
}
