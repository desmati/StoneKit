namespace System.Reflection
{
    /// <summary>
    /// Represents an object capable of emitting IL instructions using a CodeGenerator.
    /// </summary>
    public interface IEmitter
    {
        /// <summary>
        /// Emits IL instructions using the provided CodeGenerator.
        /// </summary>
        /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
        void Emit(CodeGenerator generator);
    }
}
