using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// Represents an emitter for generating IL instructions to return a value.
    /// </summary>
    public sealed class EmitReturn : IEmitterType
    {
        private readonly IEmitterType _returnValue;

        /// <summary>
        /// Initializes a new instance of the EmitReturn class.
        /// </summary>
        /// <param name="returnValue">The IEmitterType representing the return value.</param>
        /// <param name="returnType">The Type of the return value, or null to infer from returnValue.</param>
        private EmitReturn(IEmitterType returnValue, Type returnType)
        {
            // Infers the ObjectType from returnType or uses the ObjectType of returnValue
            ObjectType = returnType ?? returnValue.ObjectType;
            _returnValue = returnValue;
        }

        /// <summary>
        /// Gets the Type of the return value.
        /// </summary>
        public Type ObjectType { get; }

        /// <summary>
        /// Emits IL instructions to return the specified value.
        /// </summary>
        /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
        public void Emit(CodeGenerator generator)
        {
            // Emit instructions for the return value
            _returnValue.Emit(generator);

            // Check if explicit casting is needed before emitting the Ret instruction
            if (ObjectType == _returnValue.ObjectType)
            {
                // If no casting is needed, emit a simple Ret instruction
                generator.Emit(OpCodes.Ret);
            }
            else
            {
                // If casting is needed, cast and then emit the Ret instruction
                generator.CastType(_returnValue.ObjectType, ObjectType)
                         .Emit(OpCodes.Ret);
            }
        }

        /// <summary>
        /// Creates an instance of EmitReturn for returning the specified value.
        /// </summary>
        /// <param name="returnValue">The IEmitterType representing the return value.</param>
        /// <param name="returnType">The Type of the return value, or null to infer from returnValue.</param>
        /// <returns>An IEmitterType representing the EmitReturn instance.</returns>
        public static IEmitterType Return(IEmitterType returnValue, Type returnType = null!)
        {
            return new EmitReturn(returnValue, returnType);
        }
    }
}
