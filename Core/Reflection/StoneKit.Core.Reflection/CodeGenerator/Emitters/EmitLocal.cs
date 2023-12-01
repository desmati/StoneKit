using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// Provides utility methods for emitting IL instructions related to local variables.
    /// </summary>
    public static class EmitLocal
    {
        /// <summary>
        /// Loads the value of a local variable using IL code generation.
        /// </summary>
        /// <param name="localBuilder">The LocalBuilder representing the local variable to load.</param>
        /// <returns>An IEmitterType representing the loaded local variable value.</returns>
        public static IEmitterType Load(LocalBuilder localBuilder)
        {
            // Creates an instance of EmitLoadLocal for loading the specified local variable
            return new EmitLoadLocal(localBuilder);
        }

        /// <summary>
        /// Represents an emitter for loading the value of a local variable.
        /// </summary>
        private sealed class EmitLoadLocal : IEmitterType
        {
            private readonly LocalBuilder _localBuilder;

            /// <summary>
            /// Initializes a new instance of the EmitLoadLocal class.
            /// </summary>
            /// <param name="localBuilder">The LocalBuilder representing the local variable to load.</param>
            public EmitLoadLocal(LocalBuilder localBuilder)
            {
                _localBuilder = localBuilder;
                ObjectType = localBuilder.LocalType;
            }

            /// <summary>
            /// Gets the Type of the loaded local variable value.
            /// </summary>
            public Type ObjectType { get; }

            /// <summary>
            /// Emits IL instructions to load the value of the local variable onto the stack.
            /// </summary>
            /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
            public void Emit(CodeGenerator generator)
            {
                // Switch based on LocalIndex to emit the appropriate Ldloc instruction
                switch (_localBuilder.LocalIndex)
                {
                    case 0:
                        generator.Emit(OpCodes.Ldloc_0);
                        break;
                    case 1:
                        generator.Emit(OpCodes.Ldloc_1);
                        break;
                    case 2:
                        generator.Emit(OpCodes.Ldloc_2);
                        break;
                    case 3:
                        generator.Emit(OpCodes.Ldloc_3);
                        break;
                    default:
                        generator.Emit(OpCodes.Ldloc, _localBuilder.LocalIndex);
                        break;
                }
            }
        }
    }
}
