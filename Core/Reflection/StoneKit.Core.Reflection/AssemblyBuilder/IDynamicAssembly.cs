using System.Reflection.Emit;

namespace System.Reflection
{
    public interface IDynamicAssembly
    {
        TypeBuilder DefineType(string typeName, Type parentType);
    }
}
