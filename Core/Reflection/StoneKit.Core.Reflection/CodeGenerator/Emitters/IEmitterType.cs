namespace System.Reflection
{
    /// <summary>
    /// Represents a type that can be emitted using an IL code generator.
    /// </summary>
    public interface IEmitterType : IEmitter
    {
        /// <summary>
        /// Gets the Type of the object to be emitted.
        /// </summary>
        Type ObjectType { get; }
    }
}
