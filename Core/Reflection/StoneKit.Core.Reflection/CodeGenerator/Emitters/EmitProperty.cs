namespace System.Reflection
{
    /// <summary>
    /// Provides utility methods for emitting IL instructions related to properties.
    /// </summary>
    public sealed class EmitProperty
    {
        /// <summary>
        /// Loads the value of a property using IL code generation.
        /// </summary>
        /// <param name="source">The IEmitterType representing the source object.</param>
        /// <param name="property">The PropertyInfo representing the property to load.</param>
        /// <returns>An IEmitterType representing the loaded property value.</returns>
        public static IEmitterType Load(IEmitterType source, PropertyInfo property)
        {
            return new EmitLoadProperty(source, property);
        }

        /// <summary>
        /// Stores a value in a property using IL code generation.
        /// </summary>
        /// <param name="property">The PropertyInfo representing the property to store.</param>
        /// <param name="targetObject">The IEmitterType representing the target object for the property.</param>
        /// <param name="value">The IEmitterType representing the value to store in the property.</param>
        /// <returns>An IEmitterType representing the stored property value.</returns>
        public static IEmitterType Store(PropertyInfo property, IEmitterType targetObject, IEmitterType value)
        {
            return new EmitStoreProperty(property, targetObject, value);
        }

        /// <summary>
        /// Represents an emitter for loading the value of a property.
        /// </summary>
        private sealed class EmitLoadProperty : IEmitterType
        {
            private readonly PropertyInfo _property;
            private readonly IEmitterType _source;

            /// <summary>
            /// Initializes a new instance of the EmitLoadProperty class.
            /// </summary>
            /// <param name="source">The IEmitterType representing the source object.</param>
            /// <param name="property">The PropertyInfo representing the property to load.</param>
            public EmitLoadProperty(IEmitterType source, PropertyInfo property)
            {
                _source = source;
                _property = property;
                ObjectType = property.PropertyType;
            }

            /// <summary>
            /// Gets the Type of the loaded property value.
            /// </summary>
            public Type ObjectType { get; }

            /// <summary>
            /// Emits IL instructions to load the value of the property.
            /// </summary>
            /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
            public void Emit(CodeGenerator generator)
            {
                // Calls the getter method and emits the instructions
                EmitMethod.Call(_property!.GetGetMethod()!, _source, null!).Emit(generator);
            }
        }

        /// <summary>
        /// Represents an emitter for storing a value in a property.
        /// </summary>
        private sealed class EmitStoreProperty : IEmitterType
        {
            private readonly IEmitterType _callMethod;

            /// <summary>
            /// Initializes a new instance of the EmitStoreProperty class.
            /// </summary>
            /// <param name="property">The PropertyInfo representing the property to store.</param>
            /// <param name="targetObject">The IEmitterType representing the target object for the property.</param>
            /// <param name="value">The IEmitterType representing the value to store in the property.</param>
            public EmitStoreProperty(PropertyInfo property, IEmitterType targetObject, IEmitterType value)
            {
                // Calls the setter method and emits the instructions
                var method = property.GetSetMethod();
                _callMethod = EmitMethod.Call(method!, targetObject, value);
                ObjectType = _callMethod.ObjectType;
            }

            /// <summary>
            /// Gets the Type of the stored property value.
            /// </summary>
            public Type ObjectType { get; }

            /// <summary>
            /// Emits IL instructions to store the value in the property.
            /// </summary>
            /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
            public void Emit(CodeGenerator generator)
            {
                // Emits the instructions for the setter method call
                _callMethod.Emit(generator);
            }
        }
    }
}
