using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// Provides utility methods for emitting IL instructions related to field operations.
    /// </summary>
    public static class EmitField
    {
        /// <summary>
        /// Loads the value of a field using IL code generation.
        /// </summary>
        /// <param name="source">The IEmitterType representing the source object containing the field.</param>
        /// <param name="field">The FieldInfo representing the field to load.</param>
        /// <returns>An IEmitterType representing the loaded field value.</returns>
        public static IEmitterType Load(IEmitterType source, FieldInfo field)
        {
            // Creates an instance of EmitLoadField for loading the specified field
            return new EmitLoadField(source, field);
        }

        /// <summary>
        /// Stores a value in a field using IL code generation.
        /// </summary>
        /// <param name="field">The FieldInfo representing the field to store the value in.</param>
        /// <param name="targetObject">The IEmitterType representing the target object containing the field.</param>
        /// <param name="value">The IEmitterType representing the value to store in the field.</param>
        /// <returns>An IEmitterType representing the stored field value.</returns>
        public static IEmitterType Store(FieldInfo field, IEmitterType targetObject, IEmitterType value)
        {
            // Creates an instance of EmitStoreField for storing a value in the specified field
            return new EmitStoreField(field, targetObject, value);
        }

        /// <summary>
        /// Represents an emitter for loading the value of a field.
        /// </summary>
        private sealed class EmitLoadField : IEmitterType
        {
            private readonly FieldInfo _field;
            private readonly IEmitterType _source;

            /// <summary>
            /// Initializes a new instance of the EmitLoadField class.
            /// </summary>
            /// <param name="source">The IEmitterType representing the source object containing the field.</param>
            /// <param name="field">The FieldInfo representing the field to load.</param>
            public EmitLoadField(IEmitterType source, FieldInfo field)
            {
                _source = source;
                _field = field;
                ObjectType = field.FieldType;
            }

            /// <summary>
            /// Gets the Type of the loaded field value.
            /// </summary>
            public Type ObjectType { get; }

            /// <summary>
            /// Emits IL instructions to load the value of the field onto the stack.
            /// </summary>
            /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
            public void Emit(CodeGenerator generator)
            {
                // Emit instructions to load the source object and then the field value
                _source.Emit(generator);
                generator.Emit(OpCodes.Ldfld, _field);
            }
        }

        /// <summary>
        /// Represents an emitter for storing a value in a field.
        /// </summary>
        private sealed class EmitStoreField : IEmitterType
        {
            private readonly FieldInfo _field;
            private readonly IEmitterType _targetObject;
            private readonly IEmitterType _value;

            /// <summary>
            /// Initializes a new instance of the EmitStoreField class.
            /// </summary>
            /// <param name="field">The FieldInfo representing the field to store the value in.</param>
            /// <param name="targetObject">The IEmitterType representing the target object containing the field.</param>
            /// <param name="value">The IEmitterType representing the value to store in the field.</param>
            public EmitStoreField(FieldInfo field, IEmitterType targetObject, IEmitterType value)
            {
                _field = field;
                _targetObject = targetObject;
                _value = value;
                ObjectType = _field.FieldType;
            }

            /// <summary>
            /// Gets the Type of the stored field value.
            /// </summary>
            public Type ObjectType { get; }

            /// <summary>
            /// Emits IL instructions to store the value in the field.
            /// </summary>
            /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
            public void Emit(CodeGenerator generator)
            {
                // Emit instructions to load the target object, the value, cast if needed, and store in the field
                _targetObject.Emit(generator);
                _value.Emit(generator);
                generator.CastType(_value.ObjectType, _field.FieldType);
                generator.Emit(OpCodes.Stfld, _field);
            }
        }
    }
}
