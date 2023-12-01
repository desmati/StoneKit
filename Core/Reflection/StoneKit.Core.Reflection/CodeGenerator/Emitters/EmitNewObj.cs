namespace System.Reflection
{
    /// <summary>
    /// Represents an emitter for creating a new object using IL code generation.
    /// </summary>
    public sealed class EmitNewObj : IEmitterType
    {
        /// <summary>
        /// Initializes a new instance of the EmitNewObj class.
        /// </summary>
        /// <param name="objectType">The Type of the object to be created.</param>
        private EmitNewObj(Type objectType)
        {
            // Set the ObjectType to the specified object type
            ObjectType = objectType;
        }

        /// <summary>
        /// Gets the Type of the object to be created.
        /// </summary>
        public Type ObjectType { get; }

        /// <summary>
        /// Emits IL instructions to create a new object using the default constructor.
        /// </summary>
        /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
        public void Emit(CodeGenerator generator)
        {
            // Get the default constructor for the specified object type
            var ctor = ObjectType.GetDefaultCtor();

            // Emit instructions to create a new object using the default constructor
            generator.EmitNewObject(ctor!);
        }

        /// <summary>
        /// Creates an instance of EmitNewObj for creating a new object of the specified type.
        /// </summary>
        /// <param name="objectType">The Type of the object to be created.</param>
        /// <returns>An IEmitterType representing the EmitNewObj instance.</returns>
        public static IEmitterType NewObj(Type objectType)
        {
            return new EmitNewObj(objectType);
        }
    }
}
