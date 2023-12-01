using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// Represents a dynamic assembly builder used for creating dynamic types.
    /// </summary>
    public class DynamicAssemblyBuilder
    {
        public const string AssemblyName = "StoneKitCoreReflectionDynamicAssemblyName";
        private static readonly DynamicAssembly _dynamicAssembly = new DynamicAssembly();

        /// <summary>
        /// Gets the instance of the dynamic assembly.
        /// </summary>
        /// <returns>The dynamic assembly instance.</returns>
        public static IDynamicAssembly Get()
        {
            return _dynamicAssembly;
        }

        private sealed class DynamicAssembly : IDynamicAssembly
        {
            private readonly ModuleBuilder _moduleBuilder;

            /// <summary>
            /// Initializes a new instance of the <see cref="DynamicAssembly"/> class.
            /// </summary>
            public DynamicAssembly()
            {
                var assemblyName = new AssemblyName(AssemblyName);
                AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                _moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name!);
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

            /// <summary>
            /// Saves the dynamic assembly.
            /// </summary>
            public void Save(bool skip = true)
            {
                if (!skip)
                {
                    _dynamicAssembly.Save();
                }
            }
        }
    }
}
