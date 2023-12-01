using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// Represents an emitter for loading a null value onto the stack using IL code generation.
    /// </summary>
    public sealed class EmitNull : IEmitterType
    {
        /// <summary>
        /// Initializes a new instance of the EmitNull class.
        /// </summary>
        private EmitNull()
        {
            // Set the ObjectType to typeof(object) since null is a reference type
            ObjectType = typeof(object);
        }

        /// <summary>
        /// Gets the Type of the null value.
        /// </summary>
        public Type ObjectType { get; }

        /// <summary>
        /// Emits IL instructions to load a null value onto the stack.
        /// </summary>
        /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
        public void Emit(CodeGenerator generator)
        {
            // Emit the Ldnull instruction to load null onto the stack
            generator.Emit(OpCodes.Ldnull);
        }

        /// <summary>
        /// Creates an instance of EmitNull for loading a null value onto the stack.
        /// </summary>
        /// <returns>An IEmitterType representing the EmitNull instance.</returns>
        public static IEmitterType Load()
        {
            return new EmitNull();
        }
    }
}
