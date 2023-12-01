using System.Reflection.Emit;

namespace System.Reflection
{
    public sealed class DynamicAssembly : IDynamicAssembly
    {
        private readonly ModuleBuilder _moduleBuilder;
        private readonly AssemblyBuilder _assemblyBuilder;
        private readonly AssemblyName _assemblyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicAssembly"/> class.
        /// </summary>
        public DynamicAssembly(string assemblyName)
        {
            _assemblyName = new AssemblyName(assemblyName);
            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.Run);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_assemblyName.Name!);
        }

        /// <summary>
        /// Defines a new type in the dynamic assembly.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <param name="parentType">The parent type.</param>
        /// <returns>The type builder.</returns>
        public TypeBuilder DefineType(string typeName, Type parentType)
        {
            return _moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, parentType, null);
        }
    }
}
