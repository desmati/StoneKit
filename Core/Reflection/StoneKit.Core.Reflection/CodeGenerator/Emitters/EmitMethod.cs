namespace System.Reflection
{
    /// <summary>
    /// Provides utility methods for emitting IL instructions related to method calls.
    /// </summary>
    public static class EmitMethod
    {
        /// <summary>
        /// Calls a method with the specified invocation object and arguments using IL code generation.
        /// </summary>
        /// <param name="method">The MethodInfo representing the method to be called.</param>
        /// <param name="invocationObject">The IEmitterType representing the object on which the method is called.</param>
        /// <param name="arguments">The IEmitterType array representing the arguments to pass to the method.</param>
        /// <returns>An IEmitterType representing the method call.</returns>
        public static IEmitterType Call(MethodInfo method, IEmitterType invocationObject, params IEmitterType[] arguments)
        {
            return new EmitterCallMethod(method, invocationObject, arguments);
        }

        /// <summary>
        /// Calls a static method with the specified arguments using IL code generation.
        /// </summary>
        /// <param name="method">The MethodInfo representing the static method to be called.</param>
        /// <param name="arguments">The IEmitterType array representing the arguments to pass to the method.</param>
        /// <returns>An IEmitterType representing the static method call.</returns>
        public static IEmitterType CallStatic(MethodInfo method, params IEmitterType[] arguments)
        {
            return new EmitterCallMethod(method, null!, arguments);
        }

        /// <summary>
        /// Represents an emitter for calling a method using IL code generation.
        /// </summary>
        private sealed class EmitterCallMethod : IEmitterType
        {
            private readonly IEmitterType[] _arguments;
            private readonly IEmitterType _invocationObject;
            private readonly MethodInfo _method;

            /// <summary>
            /// Initializes a new instance of the EmitterCallMethod class.
            /// </summary>
            /// <param name="method">The MethodInfo representing the method to be called.</param>
            /// <param name="invocationObject">The IEmitterType representing the object on which the method is called.</param>
            /// <param name="arguments">The IEmitterType array representing the arguments to pass to the method.</param>
            public EmitterCallMethod(MethodInfo method, IEmitterType invocationObject, params IEmitterType[] arguments)
            {
                _method = method;
                _invocationObject = invocationObject;
                _arguments = arguments;
                ObjectType = _method.ReturnType;
            }

            /// <summary>
            /// Gets the Type of the result of the method call.
            /// </summary>
            public Type ObjectType { get; }

            /// <summary>
            /// Emits IL instructions to call the method with the specified invocation object and arguments.
            /// </summary>
            /// <param name="generator">The CodeGenerator to use for emitting IL instructions.</param>
            public void Emit(CodeGenerator generator)
            {
                // Calls the EmitCall method of CodeGenerator with the specified parameters
                generator.EmitCall(_method, _invocationObject, _arguments);
            }
        }
    }
}
