using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// Provides utility methods for emitting IL instructions related to method arguments.
    /// </summary>
    public static class EmitArgument
    {
        /// <summary>
        /// Loads the value of a method argument at the specified index using IL code generation.
        /// </summary>
        /// <param name="type">The Type of the method argument.</param>
        /// <param name="index">The index of the method argument to load.</param>
        /// <returns>An IEmitterType representing the loaded method argument value.</returns>
        public static IEmitterType Load(Type type, int index)
        {
            // Creates an instance of EmitLoadArgument for loading the specified method argument
            return new EmitLoadArgument(type, index);
        }

        /// <summary>
        /// Represents an emitter for loading the value of a method argument.
        /// </summary>
        private sealed class EmitLoadArgument : IEmitterType
        {
            private readonly int _index;

            /// <summary>
            /// Initializes a new instance of the EmitLoadArgument class.
            /// </summary>
            /// <param name="type">The Type of the method argument.</param>
            /// <param name="index">The index of the method argument to load.</param>
            public EmitLoadArgument(Type type, int index)
            {
                ObjectType = type;
                _index = index;
            }

            /// <summary>
            /// Gets the Type of the loaded method argument value.
            /// </summary>
            public Type ObjectType { get; }

            /// <summary>
            /// Emits IL instructions to load the value of the method argument onto the stack.
            /// </summary>
            /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
            public void Emit(CodeGenerator generator)
            {
                // Switch based on the index to emit the appropriate Ldarg instruction
                switch (_index)
                {
                    case 0:
                        generator.Emit(OpCodes.Ldarg_0);
                        break;
                    case 1:
                        generator.Emit(OpCodes.Ldarg_1);
                        break;
                    case 2:
                        generator.Emit(OpCodes.Ldarg_2);
                        break;
                    case 3:
                        generator.Emit(OpCodes.Ldarg_3);
                        break;
                    default:
                        generator.Emit(OpCodes.Ldarg, _index);
                        break;
                }
            }
        }
    }
}
