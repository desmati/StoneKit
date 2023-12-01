using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// Represents an emitter for boxing a value type using IL code generation.
    /// </summary>
    public sealed class EmitBox : IEmitterType
    {
        private readonly IEmitterType _value;

        /// <summary>
        /// Initializes a new instance of the EmitBox class.
        /// </summary>
        /// <param name="value">The IEmitterType representing the value to be boxed.</param>
        private EmitBox(IEmitterType value)
        {
            _value = value;
            ObjectType = value.ObjectType;
        }

        /// <summary>
        /// Gets the Type of the boxed value.
        /// </summary>
        public Type ObjectType { get; }

        /// <summary>
        /// Emits IL instructions to box a value type, if necessary.
        /// </summary>
        /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
        public void Emit(CodeGenerator generator)
        {
            // Emit instructions to load the value onto the stack
            _value.Emit(generator);

            // Check if the value type needs to be boxed
            if (ObjectType.IsValueType)
            {
                // Emit the Box instruction to box the value type
                generator.Emit(OpCodes.Box, ObjectType);
            }
        }

        /// <summary>
        /// Creates an instance of EmitBox for boxing the specified value.
        /// </summary>
        /// <param name="value">The IEmitterType representing the value to be boxed.</param>
        /// <returns>An IEmitterType representing the EmitBox instance.</returns>
        public static IEmitterType Box(IEmitterType value)
        {
            return new EmitBox(value);
        }
    }
}
