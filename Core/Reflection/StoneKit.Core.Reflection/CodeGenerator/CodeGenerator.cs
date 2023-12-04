using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// Represents a code generator for emitting IL instructions dynamically.
    /// </summary>
    public sealed class CodeGenerator
    {
        private readonly ILGenerator _ilGenerator;

        /// <summary>
        /// Initializes a new instance of the CodeGenerator class.
        /// </summary>
        /// <param name="ilGenerator">The ILGenerator to use for emitting instructions.</param>
        public CodeGenerator(ILGenerator ilGenerator)
        {
            _ilGenerator = ilGenerator;
        }

        /// <summary>
        /// Casts the given stack type to the target type.
        /// </summary>
        /// <param name="stackType">The current type on the stack.</param>
        /// <param name="targetType">The target type to cast to.</param>
        /// <returns>The CodeGenerator instance.</returns>
        public CodeGenerator CastType(Type stackType, Type targetType)
        {
            if (stackType == targetType)
            {
                return this;
            }

            if (!stackType.IsValueType && targetType == typeof(object))
            {
                return this;
            }

            if (stackType.IsValueType && !targetType.IsValueType)
            {
                _ilGenerator.Emit(OpCodes.Box, stackType);
            }
            else if (!stackType.IsValueType && targetType.IsValueType)
            {
                _ilGenerator.Emit(OpCodes.Unbox_Any, targetType);
            }
            else
            {
                _ilGenerator.Emit(OpCodes.Castclass, targetType);
            }

            return this;
        }

        /// <summary>
        /// Declares a local variable of the specified type.
        /// </summary>
        /// <param name="type">The type of the local variable.</param>
        /// <returns>The LocalBuilder representing the declared local variable.</returns>
        public LocalBuilder DeclareLocal(Type type)
        {
            return _ilGenerator.DeclareLocal(type);
        }

        /// <summary>
        /// Emits the specified OpCode without operands.
        /// </summary>
        /// <param name="opCode">The OpCode to emit.</param>
        public void Emit(OpCode opCode)
        {
            _ilGenerator.Emit(opCode);
        }

        /// <summary>
        /// Emits the specified OpCode with an integer operand.
        /// </summary>
        /// <param name="opCode">The OpCode to emit.</param>
        /// <param name="value">The integer operand value.</param>
        public void Emit(OpCode opCode, int value)
        {
            _ilGenerator.Emit(opCode, value);
        }

        /// <summary>
        /// Emits the specified OpCode with a Type operand.
        /// </summary>
        /// <param name="opCode">The OpCode to emit.</param>
        /// <param name="value">The Type operand value.</param>
        public void Emit(OpCode opCode, Type value)
        {
            _ilGenerator.Emit(opCode, value);
        }

        /// <summary>
        /// Emits the specified OpCode with a FieldInfo operand.
        /// </summary>
        /// <param name="opCode">The OpCode to emit.</param>
        /// <param name="value">The FieldInfo operand value.</param>
        public void Emit(OpCode opCode, FieldInfo value)
        {
            _ilGenerator.Emit(opCode, value);
        }

        /// <summary>
        /// Emits a method call instruction based on the provided MethodInfo, invocation object, and arguments.
        /// </summary>
        /// <param name="method">The MethodInfo representing the method to be called.</param>
        /// <param name="invocationObject">The object on which the method is called.</param>
        /// <param name="arguments">The arguments to pass to the method.</param>
        /// <exception cref="ArgumentException">Thrown when the number of arguments does not match the method's parameter count.</exception>
        public void EmitCall(MethodInfo? method, IEmitterType? invocationObject, params IEmitterType[] arguments)
        {
            var actualArguments = method?.GetParameters();

            // Handle null or empty arguments
            if (arguments == null)
            {
                arguments = new IEmitterType[0];
            }

            // Validate the number of arguments
            if (arguments.Length != (actualArguments?.Length ?? 0))
            {
                throw new ArgumentException("Number of arguments does not match the method's parameter count.");
            }

            // Emit the invocation object if not null
            if (invocationObject != null)
            {
                invocationObject.Emit(this);
            }

            // Emit each argument and cast its type
            for (var i = 0; i < arguments.Length; i++)
            {
                arguments[i].Emit(this);
                CastType(arguments[i].ObjectType, actualArguments![i].ParameterType);
            }

            // Emit the method call
            EmitCall(method!);
        }

        /// <summary>
        /// Emits a new object creation instruction for a given constructor.
        /// </summary>
        /// <param name="ctor">The ConstructorInfo representing the constructor to be called.</param>
        public void EmitNewObject(ConstructorInfo ctor)
        {
            _ilGenerator.Emit(OpCodes.Newobj, ctor);
        }

        private void EmitCall(MethodInfo method)
        {
            // Emit either Call or Callvirt based on whether the method is virtual
            if (method.IsVirtual)
            {
                _ilGenerator.Emit(OpCodes.Callvirt, method);
            }
            else
            {
                _ilGenerator.Emit(OpCodes.Call, method);
            }
        }
    }
}
