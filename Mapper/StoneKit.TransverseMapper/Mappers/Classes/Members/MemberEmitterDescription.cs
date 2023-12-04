using StoneKit.TransverseMapper.Common.Caches;

using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Classes.Members
{
    /// <summary>
    /// Represents a description of a member emitter, including the emitter itself and an optional mapper cache.
    /// </summary>
    internal sealed class MemberEmitterDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberEmitterDescription"/> class.
        /// </summary>
        /// <param name="emitter">The emitter associated with the member.</param>
        /// <param name="mappers">The optional mapper cache associated with the member.</param>
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
        /// Gets or sets the optional mapper cache associated with the member.
        /// </summary>
        public Maybe<MapperCache> MapperCache { get; private set; }

        /// <summary>
        /// Adds a mapper cache value to the description.
        /// </summary>
        /// <param name="value">The mapper cache value to add.</param>
        public void AddMapper(MapperCache value)
        {
            MapperCache = value.ToMaybe();
        }
    }
}
