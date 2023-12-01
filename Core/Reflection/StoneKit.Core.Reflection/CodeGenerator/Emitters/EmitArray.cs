using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// Provides utility methods for emitting IL instructions related to array operations.
    /// </summary>
    public static class EmitArray
    {
        /// <summary>
        /// Loads the value from an array at the specified index using IL code generation.
        /// </summary>
        /// <param name="array">The IEmitterType representing the array.</param>
        /// <param name="index">The index from which to load the value.</param>
        /// <returns>An IEmitterType representing the loaded array element.</returns>
        public static IEmitterType Load(IEmitterType array, int index)
        {
            // Creates an instance of EmitLoadArray for loading the specified array element
            return new EmitLoadArray(array, index);
        }

        /// <summary>
        /// Represents an emitter for loading a value from an array.
        /// </summary>
        private sealed class EmitLoadArray : IEmitterType
        {
            private readonly IEmitterType _array;
            private readonly int _index;

            /// <summary>
            /// Initializes a new instance of the EmitLoadArray class.
            /// </summary>
            /// <param name="array">The IEmitterType representing the array.</param>
            /// <param name="index">The index from which to load the value.</param>
            public EmitLoadArray(IEmitterType array, int index)
            {
                _array = array;
                _index = index;
                ObjectType = array!.ObjectType!.GetElementType()!;
            }

            /// <summary>
            /// Gets the Type of the loaded array element.
            /// </summary>
            public Type ObjectType { get; }

            /// <summary>
            /// Emits IL instructions to load the value from the array at the specified index.
            /// </summary>
            /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
            public void Emit(CodeGenerator generator)
            {
                // Emit instructions to load the array onto the stack
                _array.Emit(generator);

                // Switch based on the index to emit the appropriate Ldc_I4 instruction
                switch (_index)
                {
                    case 0:
                        generator.Emit(OpCodes.Ldc_I4_0);
                        break;
                    case 1:
                        generator.Emit(OpCodes.Ldc_I4_1);
                        break;
                    case 2:
                        generator.Emit(OpCodes.Ldc_I4_2);
                        break;
                    case 3:
                        generator.Emit(OpCodes.Ldc_I4_3);
                        break;
                    default:
                        generator.Emit(OpCodes.Ldc_I4, _index);
                        break;
                }

                // Emit instruction to load the array element onto the stack
                generator.Emit(OpCodes.Ldelem, ObjectType);
            }
        }
    }
}
