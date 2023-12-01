using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// Represents an emitter for working with local variables using IL code generation.
    /// </summary>
    public sealed class EmitLocalVariable : IEmitterType
    {
        private readonly Maybe<LocalBuilder> _localBuilder;

        /// <summary>
        /// Initializes a new instance of the EmitLocalVariable class.
        /// </summary>
        /// <param name="localBuilder">The LocalBuilder representing the local variable.</param>
        private EmitLocalVariable(LocalBuilder localBuilder)
        {
            // Use Maybe to handle cases where localBuilder is null
            _localBuilder = localBuilder.ToMaybe();
            ObjectType = localBuilder.LocalType;
        }

        /// <summary>
        /// Gets the Type of the local variable.
        /// </summary>
        public Type ObjectType { get; }

        /// <summary>
        /// Emits IL instructions for working with the local variable, including loading its address and initializing it if it's a value type.
        /// </summary>
        /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
        public void Emit(CodeGenerator generator)
        {
            // Use Maybe to handle cases where localBuilder is null
            _localBuilder.Where(x => x.LocalType.IsValueType)
                         .Perform(x => generator.Emit(OpCodes.Ldloca, x.LocalIndex))
                         .Perform(x => generator.Emit(OpCodes.Initobj, x.LocalType));
        }

        /// <summary>
        /// Creates an instance of EmitLocalVariable for working with the specified local variable.
        /// </summary>
        /// <param name="localBuilder">The LocalBuilder representing the local variable.</param>
        /// <returns>An IEmitterType representing the EmitLocalVariable instance.</returns>
        public static IEmitterType Declare(LocalBuilder localBuilder)
        {
            return new EmitLocalVariable(localBuilder);
        }
    }
}
